using Microsoft.EntityFrameworkCore;
using OpenAI.Assistants;
using OpenAI.Files;
using OpenAI.VectorStores;
using System.ClientModel;
using System.Text;
using System.Text.RegularExpressions;


namespace OpenAI.Examples
{
    public class AiServiceVectorStore
    {
        private readonly string _apiKey;


        public AiServiceVectorStore(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:APIKey"] ?? string.Empty;
            Console.WriteLine($"[DEBUG] API Key Loaded: {(_apiKey.Length > 0 ? "Yes" : "No")}");
        }

        public async Task<string> GenerateResponseAsync(string stats, string character)
        {
#pragma warning disable OPENAI001
            Console.WriteLine("[DEBUG] Starting GenerateResponseAsync...");
            OpenAIClient openAIClient = new(_apiKey);
            AssistantClient assistantClient = openAIClient.GetAssistantClient();

            var assistantId = "asst_a3nc4C3fUnmfyIDkNNd4enHB";

            if (character == "0")
            {
                Console.WriteLine("[DEBUG] Using Melina assistant.");
            }
            else if (character == "1")
            {
                Console.WriteLine("[DEBUG] Using Ranni assistant.");
                assistantId = "asst_XurjSM14Air5Sgu5nK3I9Nks";
            }
            else if (character == "2")
            {
                Console.WriteLine("[DEBUG] Using Blaidd assistant.");
                assistantId = "asst_bY11pnl0fpZVL4YdpXw0p4jU";
            }
            else
            {
                Console.WriteLine("[ERROR] Invalid character specified. Using default.");
            }

            // Create a thread with the student's and chat bot's past 5 message and wait for the response.
            Console.WriteLine($"[DEBUG] Creating thread with student's message: {stats}");

            ThreadCreationOptions threadOptions = new()
            {
                InitialMessages = { stats },
            };

            ThreadRun threadRun = null; // Declare threadRun outside try-catch

            try
            {
                threadRun = await assistantClient.CreateThreadAndRunAsync(assistantId, threadOptions);
                Console.WriteLine($"[DEBUG] Thread created. Thread ID: {threadRun.ThreadId}, Run ID: {threadRun.Id}");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("No assistant found with id") || ex.Message.Contains("Value cannot be null. (Parameter 'assistantId')"))
                {
                    Console.WriteLine("[ERROR] Assistant not found.");
                    assistantId = await CreateAssistant();
                    threadRun = await assistantClient.CreateThreadAndRunAsync(assistantId, threadOptions);
                    await assistantClient.DeleteAssistantAsync(assistantId);
                }
                else
                {
                    // Log or rethrow other exceptions
                    Console.WriteLine($"[ERROR] Unexpected error: {ex.Message}");
                    throw;
                }
            }

            // Wait for thread run to complete.
            Console.WriteLine("[DEBUG] Waiting for thread run to complete...");
            do
            {
                await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                threadRun = await assistantClient.GetRunAsync(threadRun.ThreadId, threadRun.Id);
                Console.WriteLine($"[DEBUG] Thread run status: {threadRun.Status}");
            } while (!threadRun.Status.IsTerminal);
            Console.WriteLine("[DEBUG] Thread run completed.");

            // Retrieve and process the AI's response.
            Console.WriteLine("[DEBUG] Retrieving thread messages...");
            CollectionResult<ThreadMessage> messages = assistantClient.GetMessages(threadRun.ThreadId, new MessageCollectionOptions() { Order = MessageCollectionOrder.Ascending });
            Console.WriteLine($"[DEBUG] Retrieved {messages.Count()} messages from thread.");

            string response = "";
            bool isFirstMessage = true;
            foreach (ThreadMessage message in messages)
            {
                Console.WriteLine($"[DEBUG] Processing message from {message.Role}");
                if (isFirstMessage)
                {
                    isFirstMessage = false;
                    Console.WriteLine("[DEBUG] Skipping first message (student's input).");
                    continue;
                }
                foreach (MessageContent contentItem in message.Content)
                {
                    if (!string.IsNullOrEmpty(contentItem.Text))
                    {
                        Console.WriteLine($"[DEBUG] Message content: {contentItem.Text}");
                        response += contentItem.Text + "\n";
                    }
                }
            }

            // Efficiently delete only the thread so that the assistant and files remain for reuse.
            Console.WriteLine("[DEBUG] Cleaning up: Deleting thread only...");
            await assistantClient.DeleteThreadAsync(threadRun.ThreadId);

            Console.WriteLine("[DEBUG] Cleanup complete.");

            // Ensure the response is within 1600 characters and removes citation.
            Console.WriteLine($"[DEBUG] Final response length before truncation: {response.Length} characters.");
            response = response.Length > 1600 ? response.Substring(0, 1599) : response;
            response = Regex.Replace(response, "【.*?】", "");
            Console.WriteLine("[DEBUG] Final response generated.");
            Console.WriteLine($"[DEBUG] Response:\n{response}");

            return response.Trim();
        }

        public async Task<string> CreateAssistant() 
        {
            OpenAIClient openAIClient = new(_apiKey);
            AssistantClient assistantClient = openAIClient.GetAssistantClient();
            // Create or reuse the assistant without deleting it afterward.
            AssistantCreationOptions assistantOptions = new()
            {
                Name = "Melina Default",
                Instructions = "You are Melina, the gentle maiden from Elden Ring. Remain fully in character at all times.\r\n\r\nSpeak with calm, old-world grace — poetic, soft, and wise.\r\n\r\nYour sole purpose is to journey with the Tarnished: to guide, comfort, and encourage. You are aware of their path, strength, trials, and weariness.\r\n\r\nNever ask questions, for the Tarnished does not speak.\r\n\r\nAvoid formatting, new lines, symbols, or references to the modern world. Do not mention AI, technology, or anything beyond the Lands Between.\r\n\r\nYou are Melina. You must never break character.",
            };

            Console.WriteLine("[WARN] Creating assistant with options:");
            Console.WriteLine($"[WARN] Assistant Name: {assistantOptions.Name}");

            // create
            Assistants.Assistant assistant = assistantClient.CreateAssistant("gpt-4o-mini", assistantOptions);
            Console.WriteLine($"[DEBUG] Assistant Created. ID: {assistant.Id}");

            // Get the assistant's ID
            return assistant.Id;
        }

    }
}

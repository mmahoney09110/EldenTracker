using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using OpenAI.Examples;
using OpenAIServer.Data;
using OpenAIServer.Entities;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MaidenServer.Controllers
{
    [ApiController]
    [Route("api")]
    public class WebhookController : ControllerBase
    {
        private readonly AiServiceVectorStore _AIService;
        private readonly ILogger _logger;
        private readonly OpenAIServerContext _context;

        public WebhookController(AiServiceVectorStore AIService, ILogger<WebhookController> logger, OpenAIServerContext context)
        {
            _AIService = AIService;
            _logger = logger;
            _context = context;
        }

        // Endpoint to handle incoming SMS messages
        [HttpPost("response")]
        public async Task<IActionResult> IncomingRequest([FromForm] string Body)
        {
            _logger.LogInformation("Incoming request received with Body: {Body}", Body);

            try
            {
                _ = Task.Run(() => ParseAndStoreData(Body));
                var response = await _AIService.GenerateResponseAsync(Body);
                _logger.LogInformation("AI response: {Response}", response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating response");
                return BadRequest(ex.Message);
            }
        }

        public async Task ParseAndStoreData(string input)
        {
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            //var events = 0;
            //Check for event
           // if (lines[0].StartsWith("Event", StringComparison.OrdinalIgnoreCase))
            //{
                //string eventMessage = lines[0];
                //var eventLog = new EventLog
                //{
                //    Message = $"Event detected: {eventMessage}",
                //    Timestamp = DateTime.UtcNow
                //};

                //_context.EventLogs.Add(eventLog);

                // counter 
               // events++;
            //}

            //Parse stats

            //find or create a new ERStats object
            var playerName = "";
            foreach (var line in lines)
            {
                if (line.StartsWith("Player Name:"))
                {
                    playerName = ParseString(line);
                    _logger.LogInformation($"Found player name: {line}");
                    break;
                }
            }

            var existingStats = await _context.ERStats
                        .FirstOrDefaultAsync(s => s.Name == playerName);

            if(existingStats == null)
            {
                if (string.IsNullOrEmpty(playerName))
                {
                    _logger.LogError("Player name not found in the input.");
                    return;
                }
                existingStats = new ERStats
                {
                    Name = playerName,
                    ResposneCount = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.ERStats.Add(existingStats);
            }

            foreach (var line in lines)
            {
                if (line.StartsWith("HP:")) existingStats.HP = ParseInt(line);
                else if (line.StartsWith("Great Rune Active?:")) existingStats.GreatRune = ParseInt(line);
                else if (line.StartsWith("Max HP:")) existingStats.MaxHP = ParseInt(line);
                else if (line.StartsWith("Death Count:")) existingStats.Deaths = ParseInt(line);
                else if (line.StartsWith("Player Name:")) existingStats.Name = ParseString(line);
                else if (line.StartsWith("Player level:")) existingStats.Level = ParseInt(line);
                else if (line.StartsWith("Runes:")) existingStats.Runes = ParseInt(line);
                else if (line.StartsWith("Class:")) existingStats.Class = ParseString(line);
                else if (line.StartsWith("Gender:")) existingStats.Gender = ParseString(line);
                else if (line.StartsWith("Location:")) existingStats.Location = ParseString(line);
                else if (line.StartsWith("Primary Weapon:"))
                {
                    var weapons = line.Replace("Primary Weapon:", "").Split('|');
                    existingStats.PrimaryWeapon = weapons.ElementAtOrDefault(0)?.Trim();
                    existingStats.SecondaryWeapon = weapons.ElementAtOrDefault(1)?.Replace("Secondary:", "").Trim();
                    existingStats.TertiaryWeapon = weapons.ElementAtOrDefault(2)?.Replace("Tertiary:", "").Trim();
                }
            }
            existingStats.ResposneCount = (existingStats.ResposneCount ?? 0) + 1;
            existingStats.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
        }

        private int ParseInt(string line) =>
            int.TryParse(Regex.Match(line, @"\d+").Value, out var value) ? value : 0;

        private string ParseString(string line) =>
            line.Substring(line.IndexOf(":") + 1).Trim();

    }
}

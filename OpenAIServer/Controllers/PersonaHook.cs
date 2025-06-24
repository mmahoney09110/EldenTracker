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
    [Route("persona")]
    public class PersonaHook : ControllerBase
    {
        private readonly Persona _Persona;
        private readonly ILogger _logger;
        private readonly OpenAIServerContext _context;

        public PersonaHook(Persona persona, ILogger<WebhookController> logger, OpenAIServerContext context)
        {
            _Persona = persona;
            _logger = logger;
            _context = context;
        }

        // Endpoint to handle incoming SMS messages
        [HttpPost("response")]
        public async Task<IActionResult> IncomingRequest([FromForm] string system, [FromForm] string message)
        {
            _logger.LogInformation("PERSONA request received with Body: {message}", message);

            try
            {
                var response = await _Persona.GenerateResponseAsync(system, message);
                _logger.LogInformation("PERSONA response: {Response}", response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating PERSONA response");
                return BadRequest(ex.Message);
            }
        }
    }
}

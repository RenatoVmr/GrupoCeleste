using GrupoCeleste.Models;
using GrupoCeleste.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrupoCeleste.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly ChatbotService _chatbotService;

        public ChatbotController(ChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost("message")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new ChatResponse 
                { 
                    Message = "Por favor, escribe un mensaje válido.", 
                    Success = false 
                });
            }

            try
            {
                var response = await _chatbotService.ProcessMessageAsync(request.Message, request.SessionId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ChatResponse 
                { 
                    Message = "Lo siento, ocurrió un error. Inténtalo de nuevo.", 
                    Success = false 
                });
            }
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "healthy", service = "chatbot" });
        }
    }
}
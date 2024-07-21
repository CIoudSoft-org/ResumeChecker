using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeAutoCheckker.BuissnessLogic.OpenAIServices;

namespace ResumeAutoCheckker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMessageController : ControllerBase
    {
        private readonly ISendMessageService _sendService;

        public SendMessageController(ISendMessageService sendService)
        {
            _sendService = sendService;
        }

        [HttpPost]
        public async Task<IActionResult> Send(string message)
        {
            var response = await _sendService.SendMessage(message);
            return Ok(response);
        }
    }
}

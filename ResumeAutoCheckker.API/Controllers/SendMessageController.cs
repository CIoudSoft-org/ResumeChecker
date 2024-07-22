using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeAutoCheckker.BuissnessLogic.EmailServices;
using ResumeAutoCheckker.BuissnessLogic.OpenAIServices;
using ResumeAutoCheckker.Domain.Entities;

namespace ResumeAutoCheckker.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SendMessageController : ControllerBase
    {
        private readonly ISendMessageService _sendService;
        private readonly IEmailService _emailService;

        public SendMessageController(ISendMessageService sendService, IEmailService emailService)
        {
            _sendService = sendService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Send(string message)
        {
            var response = await _sendService.SendMessage(message);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromForm] EmailModel model)
        {

            await _emailService.SendEmailAsync(model);

            return Ok("Message was sent successfully!");
        }
    }
}

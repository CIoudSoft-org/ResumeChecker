using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeAutoCheckker.BuissnessLogic.EmailServices;
using ResumeAutoCheckker.BuissnessLogic.OpenAIServices;
using ResumeAutoCheckker.Domain.Entities;

namespace ResumeAutoCheckker.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SendToEmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public SendToEmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromForm] EmailModel model)
        {

            await _emailService.SendEmailAsync(model);

            return Ok("Message was sent successfully!");
        }
    }
}

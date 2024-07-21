using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Commands;

namespace ResumeAutoCheckker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeCheckController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ResumeCheckController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> PostResume(RegisterResumeCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Resume(RegisterResumeCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}

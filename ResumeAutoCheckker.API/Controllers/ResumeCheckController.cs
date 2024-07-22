using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Commands;
using ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Queries;

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

        [HttpGet("AcceptedResumes/{index}/{size}")]
        public async Task<IActionResult> GetAcceptedResumes(int index, int size, CancellationToken cancellationToken)
        {

            var query = new GetAllAcceptedResumes()
            {
                Index = index,
                Size = size,
            };
            var response = await _mediator.Send(query, cancellationToken);

            return Ok(response);
        }


        [HttpGet("RejectedResumes/{index}/{size}")]
        public async Task<IActionResult> GetRejectedResumes(int index, int size, CancellationToken cancellationToken)
        {

            var query = new GetAllRejectedResumes()
            {
                Index = index,
                Size = size,
            };
            var response = await _mediator.Send(query, cancellationToken);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RejectOneResume(long id, CancellationToken cancellationToken)
        {
            var command = new RejectOneResumeCommand()
            {
                Id = id
            };

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpDelete("ListOfResumes")]
        public async Task<IActionResult> RejectListOfResumes(RejectListOfResumeCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using ResumeAutoCheckker.BuissnessLogic.Abstractions;
using ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Commands;
using ResumeAutoCheckker.BuissnessLogic.ViewModels;
using ResumeAutoCheckker.Domain.Enums;
namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Handlers.CommandHandlers
{
    public class AcceptResumeCommandHandler : IRequestHandler<AcceptResumeCommand, ResponseModel>
    {
        private readonly IApplicaitonDbContext _context;

        public AcceptResumeCommandHandler(IApplicaitonDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel> Handle(AcceptResumeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var resume = await _context.Resumes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (resume == null)
                {
                    return new ResponseModel()
                    {
                        Message = "Not found",
                        isSuccess = false,
                        StatusCode = 404
                    };
                }

                //email send

                resume.Status = ResumeStatus.NotResponded;

                await _context.SaveChangesAsync(cancellationToken);

                return new ResponseModel()
                {
                    Message = "Successfully sended",
                    isSuccess = true,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Message = ex.Message,
                    isSuccess = false,
                    StatusCode = 500
                };
            }
        }
    }
}


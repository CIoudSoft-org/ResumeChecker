using MediatR;
using Microsoft.EntityFrameworkCore;
using ResumeAutoCheckker.BuissnessLogic.Abstractions;
using ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Commands;
using ResumeAutoCheckker.BuissnessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Handlers.CommandHandlers
{
    public class RejectOneResumeCommandHandler(IApplicaitonDbContext context) : IRequestHandler<RejectOneResumeCommand, ResponseModel>
    {
        private readonly IApplicaitonDbContext _context = context;

        public async Task<ResponseModel> Handle(RejectOneResumeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var resume = await _context.Resumes.FirstOrDefaultAsync(x => x.Id == request.Id);

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
                _context.Resumes.Remove(resume);
                await _context.SaveChangesAsync(cancellationToken);

                return new ResponseModel()
                {
                    Message = "Successfully rejected",
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

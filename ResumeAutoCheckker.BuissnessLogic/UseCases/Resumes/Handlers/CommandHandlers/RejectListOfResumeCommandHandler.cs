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
    public class RejectListOfResumeCommandHandler(IApplicaitonDbContext context) : IRequestHandler<RejectListOfResumeCommand, ResponseModel>
    {
        private readonly IApplicaitonDbContext _context = context;
        public async Task<ResponseModel> Handle(RejectListOfResumeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var resumes = await _context.Resumes.Where(x => request.IdentityDocuments.Contains(x.Id)).ToListAsync();

                foreach (var resume in resumes)
                {
                    //email send
                    _context.Resumes.Remove(resume);
                }
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

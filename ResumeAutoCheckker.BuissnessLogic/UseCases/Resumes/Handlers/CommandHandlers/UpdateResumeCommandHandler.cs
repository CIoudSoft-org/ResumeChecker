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

namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Handlers.CommandHandlers;

public class UpdateResumeCommandHandler(IApplicaitonDbContext context) : IRequestHandler<UpdateResumeCommand, ResponseModel>
{
    private readonly IApplicaitonDbContext _context = context;

    public async Task<ResponseModel> Handle(UpdateResumeCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var resume = await _context.Resumes.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (resume == null)
                return new ResponseModel() { isSuccess = false, Message = "Not found", StatusCode = 404 };

            resume.Status = request.ResumeStatus;

            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseModel()
            {
                isSuccess = true,
                Message = "Updated",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel()
            {
                Message = ex.Message,
                StatusCode = 400,
                isSuccess = false
            };
        }
    }
}


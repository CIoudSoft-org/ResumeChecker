﻿using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualBasic;
using ResumeAutoCheckker.BuissnessLogic.Abstractions;
using ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Commands;
using ResumeAutoCheckker.BuissnessLogic.ViewModels;
using ResumeAutoCheckker.Domain.Entities;
using ResumeAutoCheckker.Domain.Enums;
using System.Diagnostics;

namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Handlers.CommandHandlers;
public class RegisterResumeCommandHandler(IApplicaitonDbContext context, IWebHostEnvironment webHostEnvironment) : IRequestHandler<RegisterResumeCommand, ResponseModel>
{
    private readonly IApplicaitonDbContext _context = context;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    public async Task<ResponseModel> Handle(RegisterResumeCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var file = request.ResumeFile;
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "WorkerResumes");
            string fileName = "";

            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                    Debug.WriteLine("Directory created successfully.");
                }

                fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                filePath = Path.Combine(_webHostEnvironment.WebRootPath, "WorkerResumes", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Message = ex.Message,
                    StatusCode = 500,
                    isSuccess = false
                };
            }


            var resume = new Resume()
            {
                Email = request.Email,
                LastName = request.LastName,
                FirstName = request.FirstName,
                Status = ResumeStatus.NotResponsed,
                ResumePath = GlobalConstants.Constants.ApiUrl + "/WorkerResumes/" + file.FileName,
            };

            await _context.Resumes.AddAsync(resume, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResponseModel()
            {
                Message = $"Resume Added",
                isSuccess = true,
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

using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ResumeAutoCheckker.BuissnessLogic.Abstractions;
using ResumeAutoCheckker.BuissnessLogic.EmailServices;
using ResumeAutoCheckker.BuissnessLogic.OpenAIServices;
using ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Commands;
using ResumeAutoCheckker.BuissnessLogic.ViewModels;
using ResumeAutoCheckker.Domain.Entities;
using ResumeAutoCheckker.Domain.Enums;
using System.Diagnostics;

namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Handlers.CommandHandlers;
public class RegisterResumeCommandHandler(IApplicaitonDbContext context, IWebHostEnvironment webHostEnvironment, ISendMessageService sendMessageService, IConfiguration configuration, IEmailService emailSender) : IRequestHandler<RegisterResumeCommand, ResponseModel>
{
    private readonly IApplicaitonDbContext _context = context;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
    private readonly ISendMessageService _sendMessageService = sendMessageService;
    private readonly IEmailService _emailSender = emailSender;
    private readonly string APIURL = configuration.GetSection("PhotoUrl:Url").Value!;
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

            var response = await _sendMessageService.TextInput(filePath);
            var email = new EmailModel()
            {
                To = response.Email,
                Subject = "Resumeingizni Ko'rib Chiqish",
                Body = $"Hurmatli {response.FullName},\r\n\r\nAssalomu alaykum!\r\n\r\nSizning Junior Full Stack lavozimiga ishga kirish uchun taqdim etgan resumeingizni ko'rib chiqish jarayonidamiz. Sizning malakangiz va tajribangiz bizning talablarimizga qanchalik mos kelishini aniqlash uchun hozirda ko'rib chiqilmoqda.\r\n\r\nYaqin orada biz siz bilan bog'lanamiz va keyingi bosqichlar haqida ma'lumot beramiz. Agar sizda qandaydir savollar bo'lsa, iltimos, biz bilan bog'laning.\r\n\r\nE'tiboringiz uchun rahmat.\r\n\r\nHurmat bilan,\r\n\r\nCloudSoft jamoasi"
            };
            await _emailSender.SendEmailAsync(email);
            
            if (response == null)
            {
                return new ResponseModel()
                {
                    Message = "There is an issue in Register resume",
                    isSuccess = false,
                    StatusCode = 500,
                };
            }
            if (response.Status == ResumeStatus.Accepted)
            {
                var resume = new Resume()
                {
                    Email = response.Email,
                    LastName = response.FullName,
                    FirstName = response.FullName,
                    Status = ResumeStatus.Accepted,
                    ResumePath = APIURL + "/WorkerResumes/" + file.FileName,
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

            else
            {
                var resume = new Resume()
                {
                    Email = response.Email,
                    LastName = response.FullName,
                    FirstName = response.FullName,
                    Status = ResumeStatus.Accepted,
                    WhyRejected = response.WhyRejected,
                    ResumePath = APIURL + "/WorkerResumes/" + file.FileName,
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

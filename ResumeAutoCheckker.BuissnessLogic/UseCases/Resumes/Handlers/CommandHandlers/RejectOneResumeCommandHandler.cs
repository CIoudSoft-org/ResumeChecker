using MediatR;
using Microsoft.EntityFrameworkCore;
using ResumeAutoCheckker.BuissnessLogic.Abstractions;
using ResumeAutoCheckker.BuissnessLogic.EmailServices;
using ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Commands;
using ResumeAutoCheckker.BuissnessLogic.ViewModels;
using ResumeAutoCheckker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Rpc.Context.AttributeContext.Types;

namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Handlers.CommandHandlers
{
    public class RejectOneResumeCommandHandler(IApplicaitonDbContext context, IEmailService emailSevice) : IRequestHandler<RejectOneResumeCommand, ResponseModel>
    {
        private readonly IApplicaitonDbContext _context = context;
        private readonly IEmailService _emailSender = emailSevice;
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

                var email = new EmailModel()
                {
                    To = resume.Email,
                    Subject = "Resumeingizni Ko'rib Chiqish",
                    Body = $"Hurmatli {resume.FirstName},\r\n\r\nAssalomu alaykum!\r\n\r\nSizning Junior Full Stack lavozimiga ishga kirish uchun taqdim etgan resumeingizni ko'rib chiqish jarayonidamiz. Sizning malakangiz va tajribangiz bizning talablarimizga qanchalik mos kelishini aniqlash uchun hozirda ko'rib chiqilmoqda.\r\n\r\nYaqin orada biz siz bilan bog'lanamiz va keyingi bosqichlar haqida ma'lumot beramiz. Agar sizda qandaydir savollar bo'lsa, iltimos, biz bilan bog'laning.\r\n\r\nE'tiboringiz uchun rahmat.\r\n\r\nHurmat bilan,\r\n\r\nCloudSoft jamoasi"
                };

                await _emailSender.SendEmailAsync(email);
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

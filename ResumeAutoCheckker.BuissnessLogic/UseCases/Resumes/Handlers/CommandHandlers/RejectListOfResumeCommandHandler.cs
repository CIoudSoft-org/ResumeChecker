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

namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Handlers.CommandHandlers
{
    public class RejectListOfResumeCommandHandler(IApplicaitonDbContext context, IEmailService emailService) : IRequestHandler<RejectListOfResumeCommand, ResponseModel>
    {
        private readonly IApplicaitonDbContext _context = context;
        private readonly IEmailService _emailService = emailService;
        public async Task<ResponseModel> Handle(RejectListOfResumeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var resumes = await _context.Resumes.Where(x => request.IdentityDocuments.Contains(x.Id)).ToListAsync();

                foreach (var resume in resumes)
                {
                    EmailModel email = new EmailModel()
                    {
                        Body = $"Hurmatli {resume.FirstName},\r\n\r\nAssalomu alaykum!\r\n\r\nSizning Junior Full Stack lavozimiga ishga kirish uchun taqdim etgan resumeingizni ko'rib chiqib, afsuski, hozirgi vaqtda boshqa nomzodlar bilan davom etishga qaror qildik.\r\n\r\nSabab: {resume.WhyRejected}\r\n\r\nSizga kelajakda omad tilaymiz va sizning resumeingizni kelajakdagi bo'sh ish o'rinlari uchun saqlab qolamiz. Agar bizning boshqa lavozimlarimizga qiziqish bildirsangiz, iltimos, bizning veb-saytimizga tashrif buyuring va ariza topshiring.\r\n\r\nE'tiboringiz uchun rahmat.\r\n\r\nHurmat bilan,\r\n\r\nCloudSoft jamoasi",
                        Subject = "Resumeingizni Ko'rib Chiqish Natijalari",
                        To = resume.Email
                    };
                    await _emailService.SendEmailAsync(email);
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

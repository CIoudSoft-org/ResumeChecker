using MediatR;
using Microsoft.EntityFrameworkCore;
using ResumeAutoCheckker.BuissnessLogic.Abstractions;
using ResumeAutoCheckker.BuissnessLogic.EmailServices;
using ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Commands;
using ResumeAutoCheckker.BuissnessLogic.ViewModels;
using ResumeAutoCheckker.Domain.Entities;
using ResumeAutoCheckker.Domain.Enums;
namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Handlers.CommandHandlers
{
    public class AcceptResumeCommandHandler : IRequestHandler<AcceptResumeCommand, ResponseModel>
    {
        private readonly IApplicaitonDbContext _context;
        private readonly IEmailService _emailService;

        public AcceptResumeCommandHandler(IApplicaitonDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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

                EmailModel email = new EmailModel()
                {
                    Body = $"Hurmatli {resume.FirstName},\r\n\r\nAssalomu alaykum!\r\n\r\nSizning Junior Full Stack lavozimiga ishga kirish uchun taqdim etgan resumeingizni ko'rib chiqib, sizni ishga qabul qilishga qaror qildik. Sizning malakangiz va tajribangiz bizning talablarimizga juda mos keladi.\r\n\r\nSizning ishga kirishingiz uchun keyingi bosqichlar haqida ma'lumotlarni tez orada sizga yuboramiz. Agar sizda qandaydir savollar bo'lsa, iltimos, biz bilan bog'laning.\r\n\r\nE'tiboringiz uchun rahmat.\r\n\r\nHurmat bilan,\r\n\r\nCloudSoft jamoasi",
                    Subject = "Ishga Qabul Qilinganingiz Haqida",
                    To = resume.Email
                };
                await _emailService.SendEmailAsync(email);
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


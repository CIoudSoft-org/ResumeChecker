using ResumeAutoCheckker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.BuissnessLogic.EmailServices
{
    public interface IEmailService
    {
        public Task SendEmailAsync(EmailModel model);
    }
}

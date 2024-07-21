using MediatR;
using ResumeAutoCheckker.BuissnessLogic.ViewModels;
using ResumeAutoCheckker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Commands
{
    public class UpdateResumeCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
        public ResumeStatus ResumeStatus { get; set; }
    }
}

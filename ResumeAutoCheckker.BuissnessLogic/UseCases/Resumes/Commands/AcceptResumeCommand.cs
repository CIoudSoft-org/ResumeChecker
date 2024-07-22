using MediatR;
using ResumeAutoCheckker.BuissnessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Commands
{
    public class AcceptResumeCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
    }
}

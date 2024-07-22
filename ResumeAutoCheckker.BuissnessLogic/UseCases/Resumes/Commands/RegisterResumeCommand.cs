using MediatR;
using Microsoft.AspNetCore.Http;
using ResumeAutoCheckker.BuissnessLogic.ViewModels;

namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Commands
{
    public class RegisterResumeCommand : IRequest<ResponseModel>
    {
        public IFormFile ResumeFile { get; set; }
    }
}

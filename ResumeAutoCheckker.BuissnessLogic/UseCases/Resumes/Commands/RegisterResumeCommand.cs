using MediatR;
using Microsoft.AspNetCore.Http;
using ResumeAutoCheckker.BuissnessLogic.ViewModels;

namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Commands
{
    public class RegisterResumeCommand : IRequest<ResponseModel>
    {
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IFormFile ResumeFile { get; set; }
    }
}

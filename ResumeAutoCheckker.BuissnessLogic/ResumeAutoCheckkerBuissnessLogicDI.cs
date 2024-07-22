using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using OpenAI_API;
using ResumeAutoCheckker.BuissnessLogic.OpenAIServices;
using ResumeAutoCheckker.BuissnessLogic.EmailServices;

namespace ResumeAutoCheckker.BuissnessLogic
{
    public static class ResumeAutoCheckkerBuissnessLogicDI
    {
        public static IServiceCollection AddResumeAutoCheckkerBuissnessLogicDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISendMessageService, SendMessageService>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}

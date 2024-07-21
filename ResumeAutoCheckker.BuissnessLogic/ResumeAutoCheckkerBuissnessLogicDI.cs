using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using OpenAI_API;
using ResumeAutoCheckker.BuissnessLogic.OpenAIServices;

namespace ResumeAutoCheckker.BuissnessLogic
{
    public static class ResumeAutoCheckkerBuissnessLogicDI
    {
        public static IServiceCollection AddResumeAutoCheckkerBuissnessLogicDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IOpenAIAPI, OpenAIAPI>(provider =>
            {
                var apiKey = configuration.GetSection("OpenAISettings:ApiKey").Value;
                return new OpenAIAPI(apiKey);
            });
            
            services.AddScoped<ISendMessageService, SendMessageService>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}

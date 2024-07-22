using OpenAI_API;
using ResumeAutoCheckker.BuissnessLogic;
using ResumeAutoCheckker.Infrastructure;

namespace ResumeAutoCheckker.API
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services, ILoggingBuilder Logging)
        {

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            services.AddResumeAutoCheckkerBuissnessLogicDI(configRoot);
            services.AddResumeAutoCheckkerInfrastructureDI(configRoot);

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddSingleton<IOpenAIAPI, OpenAIAPI>(provider =>
            {
                var apiKey = configRoot.GetSection("OpenAISettings:ApiKey").Value;
                return new OpenAIAPI(apiKey);
            });

        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResumeAutoCheckker.BuissnessLogic.Abstractions;
using ResumeAutoCheckker.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.Infrastructure
{
    public static class ResumeAutoCheckkerInfrastructureDI
    {
        public static IServiceCollection AddResumeAutoCheckkerInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<IApplicaitonDbContext,RezumeCheckerDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Db"));
            });

            return services;
        }
    }
}

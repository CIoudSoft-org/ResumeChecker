using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResumeAutoCheckker.BuissnessLogic.Abstractions;
using ResumeAutoCheckker.Domain.Entities;
using ResumeAutoCheckker.Domain.Entities.Auth;
using ResumeAutoCheckker.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.Infrastructure.Persistance
{
    public class RezumeCheckerDbContext : IdentityDbContext<AppHr, IdentityRole<long>, long>, IApplicaitonDbContext
    {
        public RezumeCheckerDbContext(DbContextOptions<RezumeCheckerDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        public DbSet<AppHr> Hrs { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        async ValueTask<int> IApplicaitonDbContext.SaveChangesAsync(CancellationToken cancellation)
        {
            return await base.SaveChangesAsync(cancellation);
        }
    }
}

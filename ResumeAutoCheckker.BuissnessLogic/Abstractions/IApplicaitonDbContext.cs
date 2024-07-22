using Microsoft.EntityFrameworkCore;
using ResumeAutoCheckker.Domain.Entities;
using ResumeAutoCheckker.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.BuissnessLogic.Abstractions
{
    public interface IApplicaitonDbContext
    {
        DbSet<Resume> Resumes { get; set; }

        ValueTask<int> SaveChangesAsync(CancellationToken cancellation = default!);
    }
}

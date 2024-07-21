using MediatR;
using Microsoft.EntityFrameworkCore;
using ResumeAutoCheckker.BuissnessLogic.Abstractions;
using ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Queries;
using ResumeAutoCheckker.Domain.Entities;
using ResumeAutoCheckker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Handlers.QueryHandlers
{
    public class GetAllRejectedResumesHandler(IApplicaitonDbContext context) : IRequestHandler<GetAllRejectedResumes, IEnumerable<Resume>>
    {
        private readonly IApplicaitonDbContext _context = context;

        public async Task<IEnumerable<Resume>> Handle(GetAllRejectedResumes request, CancellationToken cancellationToken)
        {
            return await _context.Resumes
                .Where(x=>x.Status == ResumeStatus.Rejected)
                    .Skip(request.Index - 1)
                        .Take(request.Size)
                            .ToListAsync(cancellationToken);
        }
    }
}

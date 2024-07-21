using MediatR;
using ResumeAutoCheckker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.BuissnessLogic.UseCases.Resumes.Queries
{
    public class GetAllRejectedResumes : IRequest<IEnumerable<Resume>>
    {
        public int Index { get; set; }
        public int Size { get; set; }
    }
}

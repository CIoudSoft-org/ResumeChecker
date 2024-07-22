using Google.Rpc;
using ResumeAutoCheckker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.BuissnessLogic.ViewModels
{
    public class AIResponceResume
    {
        public string Email { get; set; }
        public ResumeStatus Status {  get; set; }
        public string? WhyRejected { get; set; }
        public string FullName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.Domain.Entities
{
    public class EmailModel
    {
        public string To { get; set; }
        public string Subject { get; set; } = "Quick Response To Your Resume!";
        public string? Body { get; set; }
    }
}

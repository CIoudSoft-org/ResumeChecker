using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.BuissnessLogic.ViewModels
{
    public class ResponseModel
    {
        public short StatusCode { get; set; }
        public string Message { get; set; }
        public bool isSuccess { get; set; }
    }
}

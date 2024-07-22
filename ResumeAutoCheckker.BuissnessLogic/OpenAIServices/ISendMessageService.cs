using ResumeAutoCheckker.BuissnessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.BuissnessLogic.OpenAIServices
{
    public interface ISendMessageService
    {
        public Task<AIResponceResume> TextInput(string filePath, string projectId = "sturdy-dragon-429504-v9", string location = "us-central1", string publisher = "google", string model = "gemini-1.5-flash-001");
    }
}

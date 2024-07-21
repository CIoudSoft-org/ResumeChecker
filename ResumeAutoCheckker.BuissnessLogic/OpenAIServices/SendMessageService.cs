using OpenAI_API;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeAutoCheckker.BuissnessLogic.OpenAIServices
{
    public class SendMessageService : ISendMessageService
    {
        private readonly IOpenAIAPI _openAIAPI;

        public SendMessageService(IOpenAIAPI openAIAPI)
        {
            _openAIAPI = openAIAPI;
        }

        public async Task<string> SendMessage(string message)
        {
            var chat = _openAIAPI.Chat.CreateConversation();
            chat.Model = Model.DefaultChatModel;
            chat.RequestParameters.Temperature = 0;

            chat.AppendSystemMessage("You are my friend. And you should answer my questions");

            chat.AppendUserInput("Are you a boy?");
            chat.AppendExampleChatbotOutput("Yes");
            chat.AppendUserInput("A teacher is Robinzon");
            chat.AppendExampleChatbotOutput("No");

            chat.AppendUserInput(message);
            string response = await chat.GetResponseFromChatbotAsync();
            return response;

        }
    }
}

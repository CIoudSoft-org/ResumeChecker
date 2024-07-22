using Google.Cloud.AIPlatform.V1;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using System.Reflection.PortableExecutable;
using System.Text;

namespace ResumeAutoCheckker.BuissnessLogic.OpenAIServices
{
    public class SendMessageService : ISendMessageService
    {
        public async Task<string> TextInput(string filePath,
            string projectId = "sturdy-dragon-429504-v9",
            string location = "us-central1",
            string publisher = "google",
            string model = "gemini-1.5-flash-001")
        {
            string pdfText = ExtractTextFromPdf(filePath);

            var predictionServiceClient = new PredictionServiceClientBuilder
            {
                Endpoint = $"{location}-aiplatform.googleapis.com"
            }.Build();

            string starting = "If resume has ";
            string musthave = " (check all available spellings of these words, for example, if the word ASP.NET core, check for all spellings of this word like Asp.Net core, asp.net core, they all fit)";
            string ending = " then write Accepted, Why resume was rejected = Empty(just write Empty if it's Accepted here), Full Name, Email of this resume else Rejected, Why resume was rejected, Full Name, Email. (use white spaces between each answers like (e.g. Accepted Den Rov denrov13@gmail.com))";

            //Input Place where HR should write their criterias (requirements)
            string requirements = "technical skills as C#, ASP.Net Core";

            string prompt = starting + requirements + musthave + ending;

            var generateContentRequest = new GenerateContentRequest
            {
                Model = $"projects/{projectId}/locations/{location}/publishers/{publisher}/models/{model}",
                Contents =
                {
                    new Content
                    {
                        Role = "USER",
                        Parts =
                        {
                            new Part { Text = prompt },
                            new Part { Text = pdfText }
                        }
                    }
                }
            };

            GenerateContentResponse response = await predictionServiceClient.GenerateContentAsync(generateContentRequest);

            string responseText = response.Candidates[0].Content.Parts[0].Text;
            return responseText;
        }
        private string ExtractTextFromPdf(string filePath)
        {
            using (PdfReader reader = new PdfReader(filePath))
            {
                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
                return text.ToString();
            }
        }
    }
}

using Google.Cloud.AIPlatform.V1;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Extensions.Caching.Memory;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using ResumeAutoCheckker.BuissnessLogic.ViewModels;
using System.Reflection.PortableExecutable;
using System.Text;

namespace ResumeAutoCheckker.BuissnessLogic.OpenAIServices
{
    public class SendMessageService(IMemoryCache memoryCache) : ISendMessageService
    {
        private readonly IMemoryCache _memoryCache = memoryCache;

        public async Task<AIResponceResume> TextInput(string filePath,
            string projectId = "pr-430302",
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

            string requirements = _memoryCache.Get("requirements") as string ?? "technical skills as C#, ASP.Net Core";

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
            var rep = responseText.Split(' ');

            var rp = new AIResponceResume();

            if (rep[0] == "Rejected")
            {
                rp.Status = Domain.Enums.ResumeStatus.Rejected;
                rp.Email = rep[rep.Length - 2];
                rp.FullName = rep[rep.Length - 3] + " " + rep[rep.Length - 4];
                for (var i = 1; i < rep.Length - 3; i++)
                {
                    rp.WhyRejected += rep[i] + " ";
                }
            }

            else if (rep[0] == "Accepted")
            {
                rp.Status = Domain.Enums.ResumeStatus.Accepted;
                rp.Email = rep[rep.Length - 1];
                rp.FullName = rep[rep.Length - 2] + " " + rep[rep.Length - 3];
            }


            return rp;
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

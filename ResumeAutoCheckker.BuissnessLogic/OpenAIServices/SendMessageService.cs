using Google.Apis.Auth.OAuth2;
using Google.Cloud.AIPlatform.V1;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Extensions.Caching.Memory;
using ResumeAutoCheckker.BuissnessLogic.ViewModels;
using System.Text;
using Grpc.Auth;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
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

            //GoogleCredential credential = GoogleCredential.GetApplicationDefault();

            //var predictionServiceClient = new PredictionServiceClientBuilder
            //{
            //    Endpoint = $"{location}-aiplatform.googleapis.com",
            //    ChannelCredentials = credential.ToChannelCredentials()
            //}.Build();

            //string starting = "If resume has ";
            //string musthave = " (check all available spellings of these words, for example, if the word ASP.NET core, check for all spellings of this word like Asp.Net core, asp.net core, they all fit)";
            //string ending = " then write Accepted, Why resume was rejected = Empty(just write Empty if it's Accepted here), Full Name, Email of this resume else Rejected, Why resume was rejected, Full Name, Email. (use white spaces between each answers like (e.g. Accepted Den Rov denrov13@gmail.com))";


            string requirements = _memoryCache.Get("requirements") as string ?? "technical skills";
            bool allWordsPresent = true;

            string[] requiredThings = requirements.Split(", ");

            foreach (string word in requiredThings)
            {
                if (pdfText.IndexOf(word, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    allWordsPresent = false;
                    break;
                }
            }

            string email = ExtractEmail(pdfText);

            string fullName = pdfText.Split(" ")[0];

            string[] rep = allWordsPresent ? ["Accepted", $"{fullName}", $"{email}"] : ["Rejected", "Do not have all requirements", $"{fullName}", $"{email}"];


            //    string prompt = starting + requirements + musthave + ending;

            //    var generateContentRequest = new GenerateContentRequest
            //    {
            //        Model = $"projects/{projectId}/locations/{location}/publishers/{publisher}/models/{model}",
            //        Contents =
            //{
            //    new Content
            //    {
            //        Role = "USER",
            //        Parts =
            //        {
            //            new Part { Text = prompt },
            //            new Part { Text = pdfText }
            //        }
            //    }
            //}
            //    };

            //    GenerateContentResponse response = await predictionServiceClient.GenerateContentAsync(generateContentRequest);

            //    string responseText = response.Candidates[0].Content.Parts[0].Text;
            //    var rep = responseText.Split(' ');

            var rp = new AIResponceResume();

            if (rep[0] == "Rejected")
            {
                rp.Status = Domain.Enums.ResumeStatus.Rejected;
                rp.Email = rep[3];
                rp.FullName = rep[2];/* + " " + rep[rep.Length - 4];*/
                //for (var i = 1; i < rep.Length - 3; i++)
                //{
                //    rp.WhyRejected += rep[i] + " ";
                //}
                rp.WhyRejected = rep[1];
            }
            else if (rep[0] == "Accepted")
            {
                rp.Status = Domain.Enums.ResumeStatus.Accepted;
                rp.Email = rep[2];
                rp.FullName = rep[1];/* + " " + rep[rep.Length - 3];*/
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

        private string ExtractEmail(string text)
        {
            string pattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}";
            Match match = Regex.Match(text, pattern);
            if (match.Success)
            {
                return match.Value;
            }
            return "";
        }
    }
}

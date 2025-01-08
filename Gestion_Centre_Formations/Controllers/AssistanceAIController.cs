using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Xml;


public class AIRequestModel
{
    public string UserQuery { get; set; }
}

public class CourseRecommendation
{
    public string Roadmap { get; set; }
    public List<int> RecommendedCourseIds { get; set; }
}


namespace Gestion_Centre_Formations.Controllers
{
    public class AssistanceAIController : Controller
    {
        private readonly Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();
        private readonly string MISTRAL_API_KEY = "JXtrdc7dCgmXLPGR6pcq5kjZcFWq7njJ";
        private readonly string MISTRAL_API_URL = "https://api.mistral.ai/v1/chat/completions";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetRecommendations(AIRequestModel request)
        {
            try
            {
                // Get all available courses
                var courses = db.Formations
                    .Select(f => new { f.FormationID, f.Titre, f.Description, f.Categorie })
                    .ToList();

                // Prepare the prompt for Mistral AI
                var prompt = $@"Based on the user's learning goal: ""{request.UserQuery}"", 
            create a learning roadmap using only the following available courses:
            {JsonConvert.SerializeObject(courses, Newtonsoft.Json.Formatting.Indented)}
            
            Please provide:
            1. A structured learning roadmap explaining the recommended order
            2. Why each course is relevant to their goal
            3. Return the response in this exact JSON format (no \n) just pure json:
            {{
                ""roadmap"": ""detailed explanation here"",
                ""recommendedCourseIds"": [course_ids_in_recommended_order]
            }}";

                // Create HTTP client
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {MISTRAL_API_KEY}");

                    var requestBody = new
                    {
                        model = "mistral-medium",
                        messages = new[]
                        {
                    new { role = "user", content = prompt }
                },
                        temperature = 0.7
                    };

                    var content = new StringContent(
                        JsonConvert.SerializeObject(requestBody),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var response = await client.PostAsync(MISTRAL_API_URL, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"API Error: {responseString}");
                    }

                    // Log the raw response for debugging
                    System.Diagnostics.Debug.WriteLine("Raw API Response: " + responseString);

                    // Parse Mistral AI response
                    dynamic aiResponse = JsonConvert.DeserializeObject(responseString);
                    var recommendationJson = aiResponse.choices[0].message.content.ToString();

                    // Ensure the response is valid JSON
                    try
                    {
                        var recommendation = JsonConvert.DeserializeObject<CourseRecommendation>(recommendationJson);
                        return Json(recommendation);
                    }
                    catch (Newtonsoft.Json.JsonException ex)
                    {
                        throw new Exception($"JSON Parsing Error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}
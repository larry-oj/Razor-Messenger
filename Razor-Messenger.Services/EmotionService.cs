using Microsoft.AspNetCore.Http;
using Razor_Messenger.Data.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Razor_Messenger.Data;
using Razor_Messenger.Services.Models;

namespace Razor_Messenger.Services;

public class EmotionService : IEmotionService
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;
    private readonly MessengerContext _context;
    
    public EmotionService(IHttpClientFactory clientFactory,
        IConfiguration configuration,
        MessengerContext context)
    {
        _client = clientFactory.CreateClient();
        _configuration = configuration;
        _context = context;
    }

    public async Task<EmotionType> GetEmotionAsync(string message)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://twinword-emotion-analysis-v1.p.rapidapi.com/analyze/"),
            Headers =
            {
                { "X-RapidAPI-Key", _configuration.GetSection("EmotionService:Key").Value },
                { "X-RapidAPI-Host", _configuration.GetSection("EmotionService:Host").Value }
            },
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "text", message },
            }),
        };

        var emotion = "";
        try
        {
            string? body = null;
            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                body = await response.Content.ReadAsStringAsync();
            }
            var data = JsonSerializer.Deserialize<EmotionServiceResponse>(body);
            emotion = data!.EmotionsDetected.Count > 0 ? data.EmotionsDetected[0] : "neutral";
        }
        catch (Exception e)
        {
            emotion = "neutral";
        }
        
        Console.WriteLine("EMOTION: " + emotion);

        if (_context.EmotionTypes.FirstOrDefault(e => e.Name.ToLower() == emotion.ToLower()) is not { } emotionEntity)
        {
            emotionEntity = new() { Name = emotion };
            _context.EmotionTypes.Add(emotionEntity);
            await _context.SaveChangesAsync();
        }

        return emotionEntity;
    }
}
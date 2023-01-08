using System.Text.Json.Serialization;

namespace Razor_Messenger.Services.Models;

public class EmotionServiceResponse
{
    [JsonPropertyName("emotion_scores")]
    public EmotionScores EmotionScores { get; set; }

    [JsonPropertyName("emotions_detected")]
    public List<string> EmotionsDetected { get; set; }
}

public class EmotionScores
{
    [JsonPropertyName("anger")]
    public int Anger { get; set; }

    [JsonPropertyName("disgust")]
    public int Disgust { get; set; }

    [JsonPropertyName("fear")]
    public int Fear { get; set; }

    [JsonPropertyName("joy")]
    public double Joy { get; set; }

    [JsonPropertyName("sadness")]
    public double Sadness { get; set; }

    [JsonPropertyName("surprise")]
    public double Surprise { get; set; }
}
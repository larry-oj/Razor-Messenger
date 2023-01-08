using Razor_Messenger.Data.Models;

namespace Razor_Messenger.Services;

public interface IEmotionService
{
    Task<EmotionType> GetEmotionAsync(string message);
}
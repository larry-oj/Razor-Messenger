using Razor_Messenger.Data.Models;

namespace Razor_Messenger.Services;

public interface IMessageService
{
    Task<Message> SendMessageAsync(string sender, string receiver, string message);
    IEnumerable<Message> GetLastMessages(string participantOne, string participantTwo, int take);
    IEnumerable<Message> GetLastMessages(string participantOne, string participantTwo, int skip, int take);
    Task AssignEmotion(Message message, EmotionType emotion);
}
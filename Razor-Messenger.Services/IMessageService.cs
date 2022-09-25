using Razor_Messenger.Data.Models;

namespace Razor_Messenger.Services;

public interface IMessageService
{
    Task SendMessageAsync(string sender, string receiver, string message);
    IEnumerable<Message> GetMessages(string sender, string receiver, int skip, int take);
}
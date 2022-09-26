using Razor_Messenger.Data.Models;

namespace Razor_Messenger.Services;

public class MessageService : IMessageService
{
    public async Task SendMessageAsync(string sender, string receiver, string message)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Message> GetLastMessages(string participantOne, string participantTwo, int take)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Message> GetLastMessages(string participantOne, string participantTwo, int skip, int take)
    {
        throw new NotImplementedException();
    }
}
using Razor_Messenger.Data;
using Razor_Messenger.Data.Models;
using Razor_Messenger.Services.Exceptions;

namespace Razor_Messenger.Services;

public class MessageService : IMessageService
{
    private readonly MessengerContext _context;
    
    public MessageService(
        MessengerContext context)
    {
        _context = context;
    }

    public async Task SendMessageAsync(string sender, string receiver, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new EmptyMessageException();
        if (sender == receiver)
            throw new MessageToSelfException();
        
        var senderUser = _context.Users.FirstOrDefault(u => u.Username == sender)
                         ?? throw new InvalidSenderException();
        var receiverUser = _context.Users.FirstOrDefault(u => u.Username == receiver)
                           ?? throw new InvalidReceiverException();
        
        var messageEntity = new Message(senderUser, receiverUser, message);
        _context.Messages.Add(messageEntity);
        await _context.SaveChangesAsync();
    }

    public IEnumerable<Message> GetLastMessages(string participantOne, string participantTwo, int take)
    {
        return GetLastMessages(participantOne, participantTwo, 0, take);
    }

    public IEnumerable<Message> GetLastMessages(string participantOne, string participantTwo, int skip, int take)
    {
        if (participantOne == participantTwo)
            throw new MessageToSelfException();
        
        var userOne = _context.Users.FirstOrDefault(u => u.Username == participantOne) 
                      ?? throw new InvalidSenderException();
        var userTwo = _context.Users.FirstOrDefault(u => u.Username == participantTwo) 
                      ?? throw new InvalidReceiverException();

        var messages = _context.Messages
            .Where(m => (m.Sender == userOne && m.Receiver == userTwo) || 
                        (m.Receiver == userOne && m.Sender == userTwo))
            .OrderByDescending(m => m.SentAt) 
            .Skip(skip)
            .Take(take)     // IMPORTANT: TakeLast() is not supported in EF Core
            .OrderBy(m => m.SentAt);

        return messages;
    }
}
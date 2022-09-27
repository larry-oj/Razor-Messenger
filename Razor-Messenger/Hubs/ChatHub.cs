using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Razor_Messenger.Services;

namespace Razor_Messenger.Hubs;

[Authorize]
public class ChatHub : Hub<IChatClient>
{
    private readonly IMessageService _messageService;
    
    public ChatHub(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task SendMessage(string receiver, string message)
    {
        var sender = base.Context.User!.Identity!.Name;

        await _messageService.SendMessageAsync(sender, receiver, message);

        var time = $"{DateTime.UtcNow.Hour}:{DateTime.UtcNow.Minute}";
        await Clients.Caller.SendMessage(message, time);
        await Clients.User(receiver).ReceiveMessage(message, time);
    }
}
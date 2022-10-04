using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Razor_Messenger.Services;

namespace Razor_Messenger.Hubs;

[Authorize]
public class ChatHub : Hub<IChatClient>
{
    private readonly IMessageService _messageService;
    private readonly IHubContext<UserListHub, IUserListClient> _userListHub;
    
    public ChatHub(IMessageService messageService, 
        IHubContext<UserListHub, IUserListClient> userListHub)
    {
        _messageService = messageService;
        _userListHub = userListHub;
    }
    
    public async Task Register(string username)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, username);
    }

    public async Task SendMessage(string receiver, string message)
    {
        var sender = base.Context.User!.Identity!.Name;

        await _messageService.SendMessageAsync(sender, receiver, message);

        var time = DateTime.UtcNow.ToString("hh:mm");
        
        await Clients.Caller.SendMessage(message, time);
        await _userListHub.Clients.Group(sender).UpdateLastMessage(receiver, message, time);
        
        await Clients.Group(receiver).ReceiveMessage(message, time);
        await _userListHub.Clients.Group(receiver).UpdateLastMessage(sender, message, time);
    }
}
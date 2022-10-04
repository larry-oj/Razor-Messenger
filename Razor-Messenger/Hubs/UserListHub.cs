using Microsoft.AspNetCore.SignalR;

namespace Razor_Messenger.Hubs;

public class UserListHub : Hub<IUserListClient>
{
    public async Task Register(string username)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, username);
    }
}
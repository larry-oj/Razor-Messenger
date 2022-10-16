using Microsoft.AspNetCore.SignalR;

namespace Razor_Messenger.Hubs;

public class UserListHub : Hub<IUserListClient>
{
    private static List<string> Users = new List<string>();
    
    public override Task OnConnectedAsync()
    {
        var user = Context.User!.Identity!.Name!;
        Users.Add(user);
        return Clients.Others.UpdateOnlineStatus(user, true);
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var user = Context.User!.Identity!.Name!;
        Users.Remove(user);
        return Clients.Others.UpdateOnlineStatus(user, false);
    }

    public Task GetOnlineUsers()
    {
        return Clients.Caller.GetOnlineUsers(Users);
    }
}
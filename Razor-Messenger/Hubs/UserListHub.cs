using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Razor_Messenger.Hubs;

public class UserListHub : Hub<IUserListClient>
{
    private static List<string> Users = new List<string>();
    
    public override Task OnConnectedAsync()
    {
        var user = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);
        Users.Add(user);
        return Clients.Others.UpdateOnlineStatus(user, true);
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var user = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);
        Users.Remove(user);
        return Clients.Others.UpdateOnlineStatus(user, false);
    }

    public Task GetOnlineUsers(IEnumerable<string> users)
    {
        var online = users.Where(u => Users.Contains(u));

        return Clients.Caller.GetOnlineUsers(online);
    }
}
using Microsoft.AspNetCore.SignalR;

namespace Razor_Messenger.Hubs;

public class UserListHub : Hub<IUserListClient>
{
}
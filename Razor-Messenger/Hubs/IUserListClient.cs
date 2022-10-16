namespace Razor_Messenger.Hubs;

public interface IUserListClient
{
    Task UpdateLastMessage(string username, string message, string messageTime, bool isSender);
    Task UpdateOnlineStatus(string username, bool isOnline);
    Task GetOnlineUsers(List<string> onlineUsers);
}
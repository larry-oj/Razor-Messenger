namespace Razor_Messenger.Hubs;

public interface IUserListClient
{
    Task UpdateLastMessage(string username, string? displayName, string message, string messageTime, bool isSender);
    Task UpdateOnlineStatus(string username, bool isOnline);
    Task GetOnlineUsers(IEnumerable<string> onlineUsers);
    Task UpdateDisplayName(string username, string displayName);
}
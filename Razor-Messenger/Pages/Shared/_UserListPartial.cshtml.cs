using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor_Messenger.Pages.Shared;

public class _UserListPartial
{
    public string Username { get; set; }
    public DateTime LastMessageTime { get; set; }
    public string LastMessageContent { get; set; }

    public _UserListPartial()
    {
    }
    
    public _UserListPartial(string username, DateTime lastMessageTime, string lastMessageContent)
    {
        Username = username;
        LastMessageTime = lastMessageTime;
        LastMessageContent = lastMessageContent;
    }
}
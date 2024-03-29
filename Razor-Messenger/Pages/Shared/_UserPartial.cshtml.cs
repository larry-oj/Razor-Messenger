﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor_Messenger.Pages.Shared;

public class _UserPartial
{
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public DateTime LastMessageTime { get; set; }
    public string LastMessageContent { get; set; }
    public bool IsSender { get; set; }

    public _UserPartial()
    {
    }
    
    public _UserPartial(string username, string displayName, DateTime lastMessageTime, string lastMessageContent, bool isSender)
    {
        Username = username;
        DisplayName = displayName;
        LastMessageTime = lastMessageTime;
        LastMessageContent = lastMessageContent;
        IsSender = isSender;
    }
}
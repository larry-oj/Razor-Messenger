﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Messenger.Data.Models;
using Razor_Messenger.Pages.Shared;
using Razor_Messenger.Services;

namespace Razor_Messenger.Pages;

[Authorize]
public class Messenger : PageModel
{
    public string? ChatUser { get; set; }
    
    public List<_UserPartial> Users { get; set; }

    private readonly UserManager<User> _userManager;
    private readonly IMessageService _messageService;

    public Messenger(UserManager<User> userManager, 
        IMessageService messageService)
    {
        _userManager = userManager;
        _messageService = messageService;
    }
    
    public void OnGet()
    {
        Users = SearchUsers(null);
    }
    
    public async Task<IActionResult> OnPostSelectUserAsync(string receiver)
    {
        var currentUserName = base.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var messages = _messageService.GetLastMessages(currentUserName, receiver, 10).ToList();
        var model = new _MessageGroupPartial(currentUserName, receiver, messages);
        
        return Partial("_MessageGroupPartial", model);
    }
    
    public async Task<IActionResult> OnPostLoadMoreMessagesAsync(string receiver, int skip)
    {
        var currentUserName = base.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var messages = _messageService.GetLastMessages(currentUserName, receiver, skip, 10).ToList();
        var model = new _MessageGroupPartial(currentUserName, receiver, messages);
        
        return Partial("_MessageGroupPartial", model);
    }

    public async Task<IActionResult> OnPostSearchUsersAsync(string? query)
    {
        var model = SearchUsers(query);

        return Partial("_UserPartial", model);
    }
    
    private List<_UserPartial> SearchUsers(string? query)
    {
        var currentUserName = base.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var queryIsEmpty = string.IsNullOrEmpty(query);

        var users = queryIsEmpty
            ? _userManager.Users
                .Where(u => u.UserName != currentUserName)
                .ToList()
            : _userManager.Users
                .Where(u => u.UserName != currentUserName && 
                            (u.UserName.ToLower().Contains(query!.ToLower()) || 
                             u.DisplayName.ToLower().Contains(query.ToLower())))
                .ToList();
        
        var model = new List<_UserPartial>();
        foreach (var user in users)
        {
            var lastMessage = _messageService.GetLastMessages(currentUserName, user.UserName, 1).ToList();
            if (queryIsEmpty && lastMessage.Count == 0)
                continue;

            var isSender = false;
            if (lastMessage.Count > 0)
            {
                var msg = lastMessage[0];
                if (msg.Sender.UserName == currentUserName)
                {
                    isSender = true;
                }
            }
            model.Add(new _UserPartial
            {
                Username = user.UserName,
                DisplayName = user.DisplayName,
                LastMessageContent = lastMessage.Count > 0 ? lastMessage[0].Content : "",
                LastMessageTime = lastMessage.Count > 0 ? lastMessage[0].SentAt : new DateTime(2000, 1, 1, 0, 0, 0),
                IsSender = isSender
            });
        }

        return model;
    }
}
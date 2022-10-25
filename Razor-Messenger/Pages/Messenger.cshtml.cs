using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Messenger.Pages.Shared;
using Razor_Messenger.Services;

namespace Razor_Messenger.Pages;

[Authorize]
public class Messenger : PageModel
{
    public string? ChatUser { get; set; }
    
    public List<_UserPartial> Users { get; set; }

    private readonly IUserService _userService;
    private readonly IMessageService _messageService;

    public Messenger(IUserService userService, 
        IMessageService messageService)
    {
        _userService = userService;
        _messageService = messageService;
    }
    
    public void OnGet()
    {
        var currentUserName = base.User.FindFirstValue(ClaimTypes.NameIdentifier);

        Users = new List<_UserPartial>();
        var users = _userService.GetAllUsers(currentUserName).ToList();
        foreach (var user in users)
        {
            var lastMessage = _messageService.GetLastMessages(currentUserName, user.Username, 1).ToList();
            var isSender = false;
            if (lastMessage.Count > 0)
            {
                var msg = lastMessage[0];
                if (msg.Sender.Username == currentUserName)
                {
                    isSender = true;
                }
            }
            Users.Add(new _UserPartial
            {
                Username = user.Username,
                DisplayName = user.DisplayName,
                LastMessageContent = lastMessage.Count > 0 ? lastMessage[0].Content : "",
                LastMessageTime = lastMessage.Count > 0 ? lastMessage[0].SentAt : new DateTime(2000, 1, 1, 0, 0, 0),
                IsSender = isSender
            });
        }
    }
    
    public async Task<IActionResult> OnPostSelectUserAsync(string receiver)
    {
        var currentUserName = base.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var messages = _messageService.GetLastMessages(currentUserName, receiver, 10).ToList();
        var model = new _MessageGroupPartial(currentUserName, receiver, messages);
        
        return Partial("_MessageGroupPartial", model);
    }
}
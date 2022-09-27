using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Messenger.Pages.Shared;
using Razor_Messenger.Services;

namespace Razor_Messenger.Pages;

[Authorize]
public class Messenger : PageModel
{
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
        Users = new List<_UserPartial>();
        
        var currentUserName = base.User.Identity!.Name!;
        var users = _userService.GetAllUsers(currentUserName).ToList();
        foreach (var user in users)
        {
            var lastMessage = _messageService.GetLastMessages(currentUserName, user.Username, 1).ToList();
            Users.Add(new _UserPartial
            {
                Username = user.Username,
                LastMessageContent = lastMessage.Count > 0 ? lastMessage[0].Content : "",
                LastMessageTime = lastMessage.Count > 0 ? lastMessage[0].SentAt : new DateTime(2000, 1, 1, 0, 0, 0)
            });
        }
    }
}
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
    
    public List<_UserListPartial> Users { get; set; }
    public List<_MessagePartial>? Messages { get; set; }
    
    private readonly IUserService _userService;
    private readonly IMessageService _messageService;

    public Messenger(IUserService userService, 
        IMessageService messageService)
    {
        _userService = userService;
        _messageService = messageService;
    }
    
    public void OnGet(string? chatUser)
    {
        var currentUserName = base.User.Identity!.Name!;
        
        if (chatUser != null)
        {
            ChatUser = chatUser;
            var messages = _messageService.GetLastMessages(currentUserName, ChatUser, 10);
            Messages = new List<_MessagePartial>();
            foreach (var message in messages)
            {
                Messages.Add(new _MessagePartial(currentUserName, message));
            }
        }
        
        Users = new List<_UserListPartial>();
        var users = _userService.GetAllUsers(currentUserName).ToList();
        foreach (var user in users)
        {
            var lastMessage = _messageService.GetLastMessages(currentUserName, user.Username, 1).ToList();
            Users.Add(new _UserListPartial
            {
                Username = user.Username,
                LastMessageContent = lastMessage.Count > 0 ? lastMessage[0].Content : "",
                LastMessageTime = lastMessage.Count > 0 ? lastMessage[0].SentAt : new DateTime(2000, 1, 1, 0, 0, 0)
            });
        }
    }
}
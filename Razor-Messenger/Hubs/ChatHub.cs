using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Razor_Messenger.Services;

namespace Razor_Messenger.Hubs;

[Authorize]
public class ChatHub : Hub<IChatClient>
{
    private readonly IMessageService _messageService;
    private readonly IHubContext<UserListHub, IUserListClient> _userListHub;
    private readonly IEmotionService _emotionService;
    
    public ChatHub(IMessageService messageService, 
        IHubContext<UserListHub, IUserListClient> userListHub,
        IEmotionService emotionService)
    {
        _messageService = messageService;
        _userListHub = userListHub;
        _emotionService = emotionService;
    }

    public async Task SendMessage(string receiver, string message)
    {
        var sender = base.Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);

        var messageEntity = await _messageService.SendMessageAsync(sender, receiver, message);

        var time = DateTime.UtcNow.ToString("HH:mm");
        
        await Clients.Caller.SendMessage(message, time, messageEntity.Id.ToString());
        await _userListHub.Clients.User(sender).UpdateLastMessage(receiver, null, message, time, true);
        
        await Clients.User(receiver).ReceiveMessage(sender, message, time, messageEntity.Id.ToString());
        await _userListHub.Clients.User(receiver).UpdateLastMessage(sender, messageEntity.Sender.DisplayName, message, time, false);
        
        var emotion = await _emotionService.GetEmotionAsync(message);
        await _messageService.AssignEmotion(messageEntity, emotion);
        var color = emotion.Name switch
        {
            "joy" => "#fbd896",
            "sadness" => "#82b2f2",
            "anger" => "#de3b40",
            "neutral" => "white",
            "fear" => "#9928b5",
            "disgust" => "#00cf37",
            "surprise" => "#ff96dc",
            _ => "white"
        };
        await Clients.User(receiver).ReceiveEmotionAnalysis(messageEntity.Id.ToString(), emotion.Name, color);
    }
}
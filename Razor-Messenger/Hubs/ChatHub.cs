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
    private readonly IBlockService _blockService;
    
    public ChatHub(IMessageService messageService, 
        IHubContext<UserListHub, IUserListClient> userListHub,
        IEmotionService emotionService,
        IBlockService blockService)
    {
        _messageService = messageService;
        _userListHub = userListHub;
        _emotionService = emotionService;
        _blockService = blockService;
    }

    public async Task SendMessage(string receiver, string message)
    {
        var sender = base.Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);

        if (_blockService.IsBlocked(sender, receiver))
        {
            await Clients.Caller.ReceiveMessage(receiver, "You can't message this user because you have blocked him", "", "-1");
            return;
        }
        else if (_blockService.IsBlocked(receiver, sender))
        {
            await Clients.Caller.ReceiveMessage(receiver, "You can't message this user because he has blocked you", "", "-1");
            return;
        }
        
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

    public async Task BlockUser(string target)
    {
        var sender = base.Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);
        
        await _blockService.BlockUserAsync(sender, target);
        
        await Clients.User(sender).Blocked(sender, target);
        await Clients.User(target).Blocked(sender, target);
    }
}
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Messenger.Data.Models;

namespace Razor_Messenger.Pages.Shared;

public class _MessageGroupPartial : PageModel
{
    public List<MessagePartial> Messages { get; set; }
    
    public string Sender { get; set; }
    public string Receiver { get; set; }
    
    public _MessageGroupPartial()
    {
    }

    public _MessageGroupPartial(string sender, string receiver, List<Message> messages)
    {
        Messages = new List<MessagePartial>();
        Sender = sender;
        Receiver = receiver;
        foreach (var message in messages)
        {
            Messages.Add(new MessagePartial(sender, message));
        }
    }

    public string GetColor(Message message)
    {
        return message.Emotion!.Name switch
        {
            "joy" => "#fbd896",
            "sadness" => "#82b2f2",
            "anger" => "#de3b40",
            "neutral" => "white",
            "fear" => "#9928b5",
            "disgust" => "#00cf37",
            "surprise" => "#ff96dc"
        };
    }
}

public class MessagePartial
{
    public string CurrentUser { get; set; }
    public string Styles { get; set; }
    public Message Message { get; set; }

    public MessagePartial()
    {
    }
    
    public MessagePartial(string currentUser, Message message)
    {
        CurrentUser = currentUser;
        Message = message;
        Styles = currentUser == message.Sender.UserName ? 
            "bg-primary text-white align-self-end" : "bg-light border text-dark";
    }
}
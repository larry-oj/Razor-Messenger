using Razor_Messenger.Data.Models;

namespace Razor_Messenger.Pages.Shared;

public class _MessagePartial
{
    public string CurrentUser { get; set; }
    public string Styles { get; set; }
    public Message Message { get; set; }

    public _MessagePartial()
    {
    }
    
    public _MessagePartial(string currentUser, Message message)
    {
        CurrentUser = currentUser;
        Message = message;
        Styles = currentUser == message.Sender.Username ? 
            "bg-primary text-white align-self-end" : "bg-light border text-dark";
    }
}
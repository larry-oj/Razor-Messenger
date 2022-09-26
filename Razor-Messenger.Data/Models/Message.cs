using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Razor_Messenger.Data.Models;

public class Message
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public User Sender { get; set; }
    
    [Required]
    public User Receiver { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    public DateTime SentAt { get; set; }


    public Message()
    {
        SentAt = DateTime.UtcNow;
    }
    
    public Message(User sender, User receiver, string content)
        : this()
    {
        Sender = sender;
        Receiver = receiver;
        Content = content;
    }
}
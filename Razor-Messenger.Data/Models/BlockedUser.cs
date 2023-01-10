using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Razor_Messenger.Data.Models;

public class BlockedUser
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public User Initiator { get; set; }
    
    [Required]
    public User Target { get; set; }
}
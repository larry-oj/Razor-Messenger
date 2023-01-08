using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Razor_Messenger.Data.Models;

public class User : IdentityUser
{
    public string DisplayName { get; set; } = "";

    public List<User> BlockedUsers { get; set; }
}
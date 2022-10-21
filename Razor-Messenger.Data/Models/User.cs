using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Razor_Messenger.Data.Models;

public class User
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    public string DisplayName { get; set; }
    
    [Required] // will be hashed
    public string Password { get; set; }
    
    public string PasswordSalt { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public User()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public User(string username, string password, string passwordSalt)
        : this()
    {
        Username = username;
        DisplayName = username;
        Password = password;
        PasswordSalt = passwordSalt;
    }
    
    public User(string username, string displayName, string password, string passwordSalt)
        : this(username, password, passwordSalt)
    {
        DisplayName = displayName;
    }
}
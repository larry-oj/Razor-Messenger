using Microsoft.Extensions.Options;
using Razor_Messenger.Data;
using Razor_Messenger.Data.Models;
using Razor_Messenger.Services.Exceptions;
using Razor_Messenger.Services.Options;

namespace Razor_Messenger.Services;

public class AuthService : IAuthService
{
    private readonly SecurityOptions _securityOptions;
    private readonly MessengerContext _context;

    public AuthService(
        IOptions<SecurityOptions> securityOptions, 
        MessengerContext context)
    {
        _securityOptions = securityOptions.Value;
        _context = context;
    }

    public User Register(string username, string password)
    {
        return Register(username, username, password);
    }
    
    public User Register(string username, string displayName, string password)
    {
        if (_context.Users.Any(u => u.Username.ToLower() == username.ToLower()))
        {
            throw new UserAlreadyExistsException();
        }
        
        var salt = CreateSalt(16);
        var hash = HashPassword(password, salt);

        var user = new User(username, displayName, hash, salt);
        _context.Users.Add(user);
        _context.SaveChanges();
        
        return user;
    }

    public User Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
        if (user == null)
            throw new InvalidCredentialsException();

        var hash = HashPassword(password, user.PasswordSalt);
        if (hash != user.Password)
            throw new InvalidCredentialsException();
            
        return user;
    }
    
    public User UpdatePassword(string username, string oldPassword, string newPassword)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
        if (user == null)
            throw new InvalidCredentialsException();
        
        var oldHash = HashPassword(oldPassword, user.PasswordSalt);
        if (oldHash != user.Password)
            throw new InvalidCredentialsException();
        
        var salt = CreateSalt(16);
        var hash = HashPassword(newPassword, salt);
        user.Password = hash;
        user.PasswordSalt = salt;
        _context.SaveChanges();
        
        return user;
    }

    public string HashPassword(string password, string salt)
    {
        var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(
            password: password + _securityOptions.HashPepper, 
            salt: Convert.FromBase64String(salt),
            iterations: 10000);
        return Convert.ToBase64String(pbkdf2.GetBytes(20));
    }

    public string CreateSalt(int size)
    {
        var buff = new byte[size];
        var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(buff);
        return Convert.ToBase64String(buff);
    }
}
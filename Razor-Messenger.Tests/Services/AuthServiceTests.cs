using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Razor_Messenger.Data;
using Razor_Messenger.Services;
using Razor_Messenger.Services.Exceptions;
using Razor_Messenger.Services.Options;

namespace Razor_Messenger.Tests.Services;

public class AuthServiceTests
{
    private MessengerContext _context;
    private IOptions<SecurityOptions> _securityOptions;
    private IAuthService _authService;

    [SetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MessengerContext>()
            .UseInMemoryDatabase(databaseName: "MessengerTest")
            .Options;
        _context = new MessengerContext(dbContextOptions);
        _securityOptions = Options.Create<SecurityOptions>(new SecurityOptions
        {
            HashPepper = "TestPepper123456"
        });
        _authService = new AuthService(_securityOptions, _context);
    }

    [Test]
    public void Register_BlankUser_NoException()
    {
        var username = "shsfasf";
        var password = "TestPassword";
        
        var user = _authService.Register(username, password);
        Assert.IsNotNull(user);
        Assert.AreEqual(user.Username, username);
        Assert.AreEqual(user.DisplayName, username);
        Console.WriteLine(user.Password);
        Console.WriteLine(user.PasswordSalt);
    }
    
    [Test]
    public void Register_ExistingUser_Exception()
    {
        var username = "asfghhsd";
        var password = "TestPassword";
        
        var user = _authService.Register(username, password);
        Assert.IsNotNull(user);
        Assert.AreEqual(user.Username, username);

        Assert.Throws<UserAlreadyExistsException>(() => 
            _authService.Register(username, password));
    }

    [Test]
    public void Register_UserWithDisplayName_NoException()
    {
        var username = "adasddd";
        var password = "TestPassword";
        var displayName = "TestDisplayName";
        
        var user = _authService.Register(username, displayName, password);
        Assert.IsNotNull(user);
        Assert.AreEqual(user.Username, username);
        Assert.AreEqual(user.DisplayName, displayName);
    }

    [Test]
    public void Login_BlankUser_NoException()
    {
        var username = "sdgbdshh";
        var password = "TestPassword";
        
        var user = _authService.Register(username, password);

        var user2 = _authService.Login(username, password);
        Assert.IsNotNull(user2);
        Assert.AreEqual(user2!.Username, username);
        Assert.AreEqual(user2!.DisplayName, username);
    }

    [Test]
    public void Login_WrongPassword_Exception()
    {
        var username = "lkslmvod";
        var password = "TestPassword";
        
        _authService.Register(username, password);

        Assert.Throws<InvalidCredentialsException>(() => 
            _authService.Login(username, password + "1"));
    }
    
    [Test]
    public void Login_WrongUsername_Exception()
    {
        var username = "pkslxjsd";
        var password = "TestPassword";
        
        _authService.Register(username, password);

        Assert.Throws<InvalidCredentialsException>(() => 
            _authService.Login(username + "1", password));
    }

    [Test]
    public void UpdatePassword_RightPassword_NoException()
    {
        var username = "pkslxjs11d";
        var password = "TestPassword";
        
        var user = _authService.Register(username, password);

        var newPassword = "NewPassword";
        _authService.UpdatePassword(username, password, newPassword);
        
        var user2 = _authService.Login(username, newPassword);
        Assert.IsNotNull(user2);
        Assert.AreEqual(user2!.Username, username);
    }
    
    [Test]
    public void UpdatePassword_WrongPassword_Exception()
    {
        var username = "pkslxjsd21";
        var password = "TestPassword";
        
        var user = _authService.Register(username, password);

        var newPassword = "NewPassword";
        Assert.Throws<InvalidCredentialsException>(() => 
            _authService.UpdatePassword(username, password + "1", newPassword));
    }
    
    [Test]
    public void UpdatePassword_WrongUsername_Exception()
    {
        var username = "pkslxjsd31";
        var password = "TestPassword";
        
        var user = _authService.Register(username, password);

        var newPassword = "NewPassword";
        Assert.Throws<InvalidCredentialsException>(() => 
            _authService.UpdatePassword(username + "1", password, newPassword));
    }
}
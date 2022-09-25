using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Razor_Messenger.Data;
using Razor_Messenger.Data.Models;
using Razor_Messenger.Services;
using Razor_Messenger.Services.Exceptions;
using Razor_Messenger.Services.Options;

namespace Razor_Messenger.Tests;

public class IAuthServiceTests
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
        Console.WriteLine(user.Password);
        Console.WriteLine(user.PasswordSalt);
    }
    
    [Test]
    public void Register_BlankUser_Exception()
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
    public void Login_BlankUser_NoException()
    {
        var username = "sdgbdshh";
        var password = "TestPassword";
        
        var user = _authService.Register(username, password);

        var user2 = _authService.Login(username, password);
        Assert.IsNotNull(user2);
        Assert.AreEqual(user2!.Username, username);
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
}
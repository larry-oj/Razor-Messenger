using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Razor_Messenger.Data;
using Razor_Messenger.Data.Models;
using Razor_Messenger.Services;
using Razor_Messenger.Services.Exceptions;

namespace Razor_Messenger.Tests.Services;

[TestFixture]
public class UserServiceTests
{
    private MessengerContext _context;
    private IUserService _userService;
    
    [OneTimeSetUp]
    public void Setup()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MessengerContext>()
            .UseInMemoryDatabase(databaseName: "RazorMessenger")
            .Options;

        _context = new MessengerContext(options);
        _context.Users.AddRange(new List<User>
        {
            new User("user_one", "password", "salt"),
            new User("user_two", "password", "salt"),
            new User("user_three", "password", "salt")
        });
        _context.SaveChanges();
        _userService = new UserService(_context);
    }
    
    [Test]
    public void GetUser_ValidUser_User()
    {
        var user = _userService.GetUser("user_one");
        
        Assert.AreEqual("user_one", user.Username);
    }
    
    [Test]
    public void GetUser_InvalidUser_Exception()
    {
        Assert.Throws<UserDoesNotExistException>(() =>
            _userService.GetUser("user_four"));
    }
    
    [Test]
    public void GetAllUsers_Users()
    {
        var users = _userService.GetAllUsers();
        
        Assert.AreEqual(3, users.Count());
    }

    [Test]
    public void GetAllUsers_ExceptUser_Users()
    {
        var users = _userService.GetAllUsers("user_one");
        
        Assert.AreEqual(2, users.Count());
        Assert.IsFalse(users.Any(u => u.Username == "user_one"));
    }
    
    [Test]
    public void GetAllUsers_ExceptUser_Exception()
    {
        Assert.Throws<UserDoesNotExistException>(() =>
            _userService.GetAllUsers("user_four"));
    }
    
    [Test]
    public async Task UpdateDisplayName_ExistingUser_NoException()
    {
        Assert.AreEqual(_userService.GetUser("user_one").DisplayName, "user_one");
        var user = await _userService.UpdateUserDisplayNameAsync("user_one", "new_display_name");
        Assert.AreEqual(user!.DisplayName, "new_display_name");
    }
    
    [Test]
    public void UpdateDisplayName_InvalidUser_Exception()
    {
        Assert.Throws<UserDoesNotExistException>(() =>
            _userService.UpdateUserDisplayNameAsync("user_four", "new_display_name"));
    }
}
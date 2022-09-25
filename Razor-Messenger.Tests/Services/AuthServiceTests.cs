using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Razor_Messenger.Data;
using Razor_Messenger.Data.Models;
using Razor_Messenger.Services;
using Razor_Messenger.Services.Options;

namespace Razor_Messenger.Tests;

public class Tests
{
    private MessengerContext _context;
    private IOptions<SecurityOptions> _securityOptions;

    [SetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MessengerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new MessengerContext(dbContextOptions);
        _securityOptions = Options.Create<SecurityOptions>(new SecurityOptions
        {
            HashPepper = "TestPepper123456"
        });
    }

    [Test]
    public void Test1()
    {
        IAuthService service = new AuthService(_securityOptions, _context);
        
        var user = new User
        {
            Username = "TestUser",
            Password = "TestPassword"
        };
    }
}
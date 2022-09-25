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
public class MessageServiceTests
{
    private MessengerContext _context;
    private IMessageService _messageService;

    [OneTimeSetUp]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MessengerContext>()
            .UseInMemoryDatabase(databaseName: "MessengerMessageTest")
            .Options;
        _context = new MessengerContext(dbContextOptions);
        _context.Add(new User("user_one", "pass", "salt"));
        _context.Add(new User("user_two", "pass", "salt"));
        _context.SaveChanges();
    }

    #region SendMessageAsync

    [Test]
    public async Task SendMessageAsync_ValidData_NoException()
    {
        var msgContent = "SendMessageAsync_ValidData_NoException";
        
        await _messageService.SendMessageAsync("user_one", "user_two", msgContent);

        var condition = _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Any(m => m.Sender.Username == "user_one" && 
                      m.Receiver.Username == "user_two" && 
                      m.Content == msgContent);
        
        Assert.IsTrue(condition);
    }
    
    [Test]
    public void SendMessageAsync_MessageEmpty_Exception()
    {
        var msgContent = "";

        Assert.ThrowsAsync<EmptyMessageException>(async () =>
            await _messageService.SendMessageAsync("user_one", "user_two", msgContent));
    }

    [Test]
    public void SendMessageAsync_InvalidSender_Exception()
    {
        Assert.ThrowsAsync<InvalidSenderException>(async () =>
            await _messageService.SendMessageAsync("user_three", "user_two", "SendMessageAsync_InvalidSender_Exception"));
    }
    
    [Test]
    public void SendMessageAsync_InvalidReceiver_Exception()
    {
        Assert.ThrowsAsync<InvalidSenderException>(async () =>
            await _messageService.SendMessageAsync("user_one", "user_three", "SendMessageAsync_InvalidReceiver_Exception"));
    }
    
    [Test]
    public void SendMessageAsync_MessageToSelf_Exception()
    {
        Assert.ThrowsAsync<MessageToSelfException>(async () =>
            await _messageService.SendMessageAsync("user_one", "user_one", "SendMessageAsync_MessageToSelf_Exception"));
    }

    #endregion

    #region GetMessages

    

    #endregion
}
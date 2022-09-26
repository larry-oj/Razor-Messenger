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

    #region GetLastMessages

    public void GetLastMessages_ValidData_NoException()
    {
        var msgContent = "GetLastMessages_ValidData_NoException";
        
        _messageService.SendMessageAsync("user_one", "user_two", "one" + msgContent);
        _messageService.SendMessageAsync("user_two", "user_one", "two" + msgContent);
        _messageService.SendMessageAsync("user_one", "user_two", "three" + msgContent);
        _messageService.SendMessageAsync("user_two", "user_one", "four" + msgContent);
        
        var messages = _messageService.GetLastMessages("user_one", "user_two", 4);
        
        Assert.AreEqual(4, messages.Count());

        _context.Messages.RemoveRange(_context.Messages);
    }
    
    [Test]
    public void GetLastMessages_ValidData_ValidMessageOrder()
    {
        var msgContent = "GetLastMessages_ValidData_NoException";
        
        _messageService.SendMessageAsync("user_one", "user_two", "one" + msgContent);
        _messageService.SendMessageAsync("user_two", "user_one", "two" + msgContent);
        _messageService.SendMessageAsync("user_one", "user_two", "three" + msgContent);
        _messageService.SendMessageAsync("user_two", "user_one", "four" + msgContent);
        
        var messages = _messageService.GetLastMessages("user_one", "user_two", 4).ToArray();
        
        Assert.AreEqual(4, messages.Length);
        Assert.AreEqual("one" + msgContent, messages[0].Content);
        Assert.AreEqual("two" + msgContent, messages[1].Content);
        Assert.AreEqual("three" + msgContent, messages[2].Content);
        Assert.AreEqual("four" + msgContent, messages[3].Content);
        
        _context.Messages.RemoveRange(_context.Messages);
    }
    
    [Test]
    public void GetLastMessages_InvalidSender_Exception()
    {
        Assert.Throws<InvalidSenderException>(() =>
            _messageService.GetLastMessages("user_three", "user_two", 4));
    }
    
    [Test]
    public void GetLastMessages_InvalidReceiver_Exception()
    {
        Assert.Throws<InvalidSenderException>(() =>
            _messageService.GetLastMessages("user_one", "user_three", 4));
    }
    
    [Test]
    public void GetLastMessages_MessageToSelf_Exception()
    {
        Assert.Throws<MessageToSelfException>(() =>
            _messageService.GetLastMessages("user_one", "user_one", 4));
    }
    
    [Test]
    public void GetLastMessages_Skip_NoException()
    {
        var msgContent = "GetLastMessages_Skip_NoException";
        
        _messageService.SendMessageAsync("user_one", "user_two", "one" + msgContent);
        _messageService.SendMessageAsync("user_two", "user_one", "two" + msgContent);
        _messageService.SendMessageAsync("user_one", "user_two", "three" + msgContent);
        _messageService.SendMessageAsync("user_two", "user_one", "four" + msgContent);
        
        var messages = _messageService.GetLastMessages("user_one", "user_two", 2, 2);
        
        Assert.AreEqual(2, messages.Count());

        _context.Messages.RemoveRange(_context.Messages);
    }
    
    [Test]
    public void GetLastMessages_Skip_ValidOrder()
    {
        var msgContent = "GetLastMessages_Skip_NoException";
        
        _messageService.SendMessageAsync("user_one", "user_two", "one" + msgContent);
        _messageService.SendMessageAsync("user_two", "user_one", "two" + msgContent);
        _messageService.SendMessageAsync("user_one", "user_two", "three" + msgContent);
        _messageService.SendMessageAsync("user_two", "user_one", "four" + msgContent);
        
        var messages = _messageService.GetLastMessages("user_one", "user_two", 2, 2).ToList();

        Assert.AreEqual(2, messages.Count);
        Assert.AreEqual("three" + msgContent, messages.First().Content);
        Assert.AreEqual("four" + msgContent, messages.Last().Content);

        _context.Messages.RemoveRange(_context.Messages);
    }
    
    [Test]
    public void GetLastMessages_SkipMoreThanExist_Exception()
    {
        var msgContent = "GetLastMessages_SkipMoreThanExist_Exception";
        
        _messageService.SendMessageAsync("user_one", "user_two", "one" + msgContent);
        _messageService.SendMessageAsync("user_two", "user_one", "two" + msgContent);
        _messageService.SendMessageAsync("user_one", "user_two", "three" + msgContent);
        _messageService.SendMessageAsync("user_two", "user_one", "four" + msgContent);
        
        var messages = _messageService.GetLastMessages("user_one", "user_two", 2, 4);
        
        Assert.AreEqual(0, messages.Count());
        
        _context.Messages.RemoveRange(_context.Messages);
    }

    #endregion
}
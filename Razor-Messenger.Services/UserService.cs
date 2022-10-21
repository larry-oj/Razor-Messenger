using Razor_Messenger.Data;
using Razor_Messenger.Data.Models;
using Razor_Messenger.Services.Exceptions;

namespace Razor_Messenger.Services;

public class UserService : IUserService
{
    private readonly MessengerContext _context;

    public UserService(MessengerContext context)
    {
        _context = context;
    }
    
    public User GetUser(string username)
    {
        return _context.Users.FirstOrDefault(u => u.Username == username)
            ?? throw new UserDoesNotExistException();
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _context.Users.AsEnumerable();
    }

    public IEnumerable<User> GetAllUsers(string exception)
    {
        var user = GetUser(exception);
        return _context.Users.Where(u => u != user);
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> UpdateUserDisplayNameAsync(string username, string displayName)
    {
        var user = GetUser(username);
        user.DisplayName = displayName;
        await UpdateUserAsync(user);
        return user;
    }
}
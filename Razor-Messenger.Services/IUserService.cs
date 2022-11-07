using Razor_Messenger.Data.Models;

namespace Razor_Messenger.Services;

public interface IUserService
{
    User GetUser(string username);
    IEnumerable<User> GetAllUsers();
    IEnumerable<User> GetAllUsers(string exception);
    IEnumerable<User> GetAllUsers(string query, string except);
    Task UpdateUserAsync(User user);
    Task<User> UpdateUserDisplayNameAsync(string username, string displayName);
}
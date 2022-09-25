using Razor_Messenger.Data.Models;

namespace Razor_Messenger.Services;

public interface IAuthService
{
    Task<User> RegisterAsync(string username, string password);
    Task<User> LoginAsync(string username, string password);
    Task<string> HashPasswordAsync(string password);
}
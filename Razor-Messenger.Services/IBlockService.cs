namespace Razor_Messenger.Services;

public interface IBlockService
{
    Task BlockUserAsync(string initiator, string target);
    
    bool IsBlocked(string initiator, string target);
}
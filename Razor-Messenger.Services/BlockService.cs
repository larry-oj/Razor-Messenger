using Razor_Messenger.Data;
using Razor_Messenger.Data.Models;

namespace Razor_Messenger.Services;

public class BlockService : IBlockService
{
    private readonly MessengerContext _context;

    public BlockService(MessengerContext context)
    {
        _context = context;
    }
    
    public async Task BlockUserAsync(string initiator, string target)
    {
        var initiatorEntity = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == initiator.ToLower());
        if (initiatorEntity == null)
            throw new ArgumentException("Initiator not found");
        
        var targetEntity = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == target.ToLower());
        if (targetEntity == null)
            throw new ArgumentException("Target not found");

        var block = _context.BlockedUsers.FirstOrDefault(b => b.Initiator == initiatorEntity && b.Target == targetEntity);
        if (block == null)
        {
            block = new BlockedUser
            {
                Initiator = initiatorEntity,
                Target = targetEntity
            };
            _context.BlockedUsers.Add(block);
        }
        else
        {
            _context.BlockedUsers.Remove(block);
        }

        await _context.SaveChangesAsync();
    }

    public bool IsBlocked(string initiator, string target)
    {
        return _context.BlockedUsers.Any(b => b.Initiator.UserName.ToLower() == initiator.ToLower() && b.Target.UserName.ToLower() == target.ToLower());
    }
}
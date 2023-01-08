using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Razor_Messenger.Data.Models;

namespace Razor_Messenger.Data;

public class MessengerContext : IdentityUserContext<User>
{
    public DbSet<Message> Messages { get; set; }
    public DbSet<EmotionType> EmotionTypes { get; set; }

    public MessengerContext(DbContextOptions<MessengerContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
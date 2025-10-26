using Microsoft.EntityFrameworkCore;
using Bizcord.Web.Shared.Entities;
namespace Bizcord.Web.Data;

public class BizcordDbContext : DbContext
{
    public BizcordDbContext(DbContextOptions<BizcordDbContext> options) : base(options) { }


    public DbSet<User> Users => Set<User>();
    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<Message> Messages => Set<Message>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.UserName).IsRequired().HasMaxLength(64);
            e.Property(x => x.DisplayName).HasMaxLength(128);
        });


        modelBuilder.Entity<Channel>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(64);
            e.HasIndex(x => x.Name).IsUnique();
        });


        modelBuilder.Entity<Message>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Content).IsRequired().HasMaxLength(4000);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("AUTOUPDATE()");
            e.HasOne(x => x.User)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Channel)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.ChannelId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
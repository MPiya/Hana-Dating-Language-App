using Hana.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hana.Hana.Database.Data;

public class HanaContext : IdentityDbContext<UserIdentity>
{
    public HanaContext(DbContextOptions<HanaContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Message>(entity =>
        {
            // Configure table name
            entity.ToTable("Messages");
            
            // Configure primary key
            entity.HasKey(e => e.Id);
            
            // Configure properties
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.SocialMediaPlatform).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SocialMediaLink).IsRequired().HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Configure SenderId relationship
            entity.HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure ReceiverId relationship
            entity.HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Message> Messages { get; set; }
}

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
    }

    public DbSet<UserProfile> UserProfiles { get; set; }
}

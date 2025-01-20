using Microsoft.EntityFrameworkCore;
using FriendsApp.Models;



public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } // Users table
    public DbSet<Friend> Friends { get; set; } // Friends table
    public DbSet<Meeting> Meetings { get; set; } // Meetings table
    public DbSet<Plan> Plans { get; set; } // Plans table

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Meeting-Friend relationship
        modelBuilder.Entity<Plan>()
      .HasMany(p => p.Friends)
      .WithMany(f => f.Plans)
      .UsingEntity<Dictionary<string, object>>(
          "PlanFriends",
          j => j.HasOne<Friend>().WithMany().HasForeignKey("FriendId").OnDelete(DeleteBehavior.Cascade),
          j => j.HasOne<Plan>().WithMany().HasForeignKey("PlanId").OnDelete(DeleteBehavior.Cascade)
      );

    }


}

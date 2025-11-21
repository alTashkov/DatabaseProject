using Microsoft.EntityFrameworkCore;
using DatabaseProject.Models;

namespace DatabaseProject.Data
{
    public class SocialMediaContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server = P3RKZmqBEGRNShe\\SQLEXPRESS;Database = DatabaseProject;Trusted_Connection = True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //one to many
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //one to many
            modelBuilder.Entity<Comment>()
                .HasOne(p => p.Post)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            //one to many
            modelBuilder.Entity<Friendship>()
                .HasOne(u => u.User1)
                .WithMany(f => f.FriendshipsInitiated)
                .HasForeignKey(u => u.UserID1)
                .OnDelete(DeleteBehavior.Restrict);


            //one to many
            modelBuilder.Entity<Friendship>()
                .HasOne(u => u.User2)
                .WithMany(f => f.FriendshipsReceived)
                .HasForeignKey(u => u.UserID2)
                .OnDelete(DeleteBehavior.Restrict);

            //one to many
            modelBuilder.Entity<Post>()
                .HasOne(u => u.User)
                .WithMany(p => p.Posts)
                .HasForeignKey(u => u.UserId);

            //one to one
            modelBuilder.Entity<User>()
                .HasOne(p => p.Profile)
                .WithOne(u => u.User)
                .HasForeignKey<Profile>(p => p.UserId);

            //many to many
            modelBuilder.Entity<User>()
                .HasMany(g => g.Groups)
                .WithMany(u => u.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserGroup", 
                    j => j.HasOne<Group>().WithMany().HasForeignKey("GroupId"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId")
                );
        }
    }
}

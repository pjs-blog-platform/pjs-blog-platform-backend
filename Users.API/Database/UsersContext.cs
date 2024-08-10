using Microsoft.EntityFrameworkCore;
using Users.API.Models;

namespace Users.API.Database
{
    public class UsersContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Users

            // Id
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();

            // Username
            modelBuilder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

            // Email
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // PasswordHash
            modelBuilder.Entity<User>().Property(u => u.PasswordHash).IsRequired();

            // Fullname
            modelBuilder.Entity<User>().Property(u => u.FullName).HasMaxLength(256);

            // Bio
            modelBuilder.Entity<User>().Property(u => u.Bio).HasMaxLength(1000);

            // ProfilePictureUrl
            modelBuilder.Entity<User>().Property(u => u.ProfilePictureUrl).HasMaxLength(2048);

            // CreatedAt
            modelBuilder.Entity<User>().Property(u => u.CreatedAt).IsRequired();

            // UpdatedAt
            modelBuilder.Entity<User>().Property(u => u.UpdatedAt).IsRequired();

            // IsActive
            modelBuilder.Entity<User>().Property(u => u.IsActive).IsRequired();

            #endregion
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Posts.API.Models;

namespace Posts.API.Database
{
    public class PostsContext : DbContext
    {
        public virtual DbSet<Post> Posts { get; set; }

        public PostsContext(DbContextOptions<PostsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Posts

            // Id
            modelBuilder.Entity<Post>().HasKey(u => u.Id);
            modelBuilder.Entity<Post>().Property(u => u.Id).ValueGeneratedOnAdd();

            // AuthorId
            modelBuilder.Entity<Post>().Property(u => u.AuthorId).IsRequired();

            // Title
            modelBuilder.Entity<Post>().Property(u => u.Title).IsRequired();

            // Content
            modelBuilder.Entity<Post>().Property(u => u.Content).IsRequired();

            // Excerpt
            modelBuilder.Entity<Post>().Property(u => u.Title).HasMaxLength(500);

            // Slug
            modelBuilder.Entity<Post>().Property(u => u.Title).IsRequired().HasMaxLength(255);

            // FeaturedImageUrl
            modelBuilder.Entity<Post>().Property(u => u.FeaturedImageUrl).HasMaxLength(255);

            // CreatedAt
            modelBuilder.Entity<Post>().Property(u => u.CreatedAt).IsRequired();

            // UpdatedAt
            modelBuilder.Entity<Post>().Property(u => u.UpdatedAt).IsRequired();

            // IsActive
            modelBuilder.Entity<Post>().Property(u => u.IsActive).IsRequired();
            #endregion
        }
    }
}

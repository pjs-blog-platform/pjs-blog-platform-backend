using Microsoft.AspNetCore.Http.HttpResults;
using Posts.API.Models;
using Posts.API.Models.DTO;

namespace Posts.API.Services
{
    public class PostMapper : IPostMapper
    {
        public PostDTO  Map(Post post)
        {
            return new PostDTO
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                Title = post.Title,
                Content = post.Content,
                Excerpt = post.Excerpt,
                Slug = post.Slug,
                FeaturedImageUrl = post.FeaturedImageUrl,
                CreatedAt = post.CreatedAt.ToUniversalTime().ToString("O"),
                UpdatedAt = post.UpdatedAt.ToUniversalTime().ToString("O")
            };
        }
    }
}

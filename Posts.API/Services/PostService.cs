using Microsoft.EntityFrameworkCore;
using Posts.API.Database;
using Posts.API.Models;
using Posts.API.Models.DTO;

namespace Posts.API.Services
{
    public class PostService : IPostService
    {
        private readonly PostsContext _context;
        private readonly IPostMapper _mapper;

        public PostService(PostsContext context, IPostMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostDTO>> GetAsync()
        {
            var posts = await _context.Posts.ToListAsync();
            return posts.Select(p => _mapper.Map(p));
        }

        public async Task<PostDTO?> GetByIdAsync(int id)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return null;

            return _mapper.Map(post);
        }

        public async Task<PostDTO> CreateAsync(CreatePostDTO createPost)
        {
            if (createPost == null)
                throw new ArgumentNullException(nameof(createPost));

            if (!createPost.AuthorId.HasValue)
                throw new ArgumentException("AuthorId is required", nameof(createPost.AuthorId));

            if (string.IsNullOrWhiteSpace(createPost.Title))
                throw new ArgumentException("Title is required", nameof(createPost.Title));

            if (string.IsNullOrWhiteSpace(createPost.Content))
                throw new ArgumentException("Content is required", nameof(createPost.Content));

            if (string.IsNullOrWhiteSpace(createPost.Slug))
                throw new ArgumentException("Slug is required", nameof(createPost.Slug));

            try
            {
                var currentTime = DateTime.Now.ToUniversalTime();

                var post = new Post()
                {
                    AuthorId = createPost.AuthorId.Value,
                    Title = createPost.Title,
                    Content = createPost.Content,
                    Excerpt = createPost.Excerpt,
                    Slug = createPost.Slug,
                    FeaturedImageUrl = createPost.FeaturedImageUrl,
                    CreatedAt = currentTime,
                    UpdatedAt = currentTime,
                    IsActive = false
                };

                var userAdded = _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return _mapper.Map(post);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdatePostDTO updatePost)
        {
            var post = _context.Posts.SingleOrDefault(u => u.Id == id);

            if (post == null)
                return false;

            post.Title = !String.IsNullOrEmpty(updatePost.Title) ? updatePost.Title : post.Title;
            post.Content = !String.IsNullOrEmpty(updatePost.Content) ? updatePost.Content : post.Content;
            post.Excerpt = !String.IsNullOrEmpty(updatePost.Excerpt) ? updatePost.Excerpt : post.Excerpt;
            post.Slug = !String.IsNullOrEmpty(updatePost.Slug) ? updatePost.Slug : post.Slug;
            post.FeaturedImageUrl = !String.IsNullOrEmpty(updatePost.FeaturedImageUrl) ? updatePost.FeaturedImageUrl : post.FeaturedImageUrl;

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var post = _context.Posts.SingleOrDefault(u => u.Id == id);

            if (post == null) return false;

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

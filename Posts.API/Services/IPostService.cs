using Posts.API.Models.DTO;

namespace Posts.API.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostDTO>> GetAsync();
        Task<PostDTO?> GetByIdAsync(int id);
        Task<PostDTO> CreateAsync(CreatePostDTO createPost);
        Task<bool> UpdateAsync(int id, UpdatePostDTO updatePost);
        Task<bool> DeleteAsync(int id);
    }
}

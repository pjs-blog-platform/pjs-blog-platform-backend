using Posts.API.Models;
using Posts.API.Models.DTO;

namespace Posts.API.Services
{
    public interface IPostMapper
    {
        PostDTO Map(Post post);
    }
}

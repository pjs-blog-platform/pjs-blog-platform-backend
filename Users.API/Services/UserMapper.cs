using Users.API.Models;
using Users.API.Models.DTO;

namespace Users.API.Services
{
    public class UserMapper : IUserMapper
    {
        public UserDTO Map(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureUrl,
                CreatedAt = user.CreatedAt.ToUniversalTime().ToString("O"),
                UpdatedAt = user.UpdatedAt.ToUniversalTime().ToString("O"),
                IsActive = user.IsActive
            };
        }
    }
}

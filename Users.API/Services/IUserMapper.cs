using Users.API.Models;
using Users.API.Models.DTO;

namespace Users.API.Services
{
    public interface IUserMapper
    {
        UserDTO Map(User user);
    }
}

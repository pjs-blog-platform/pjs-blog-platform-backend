using Users.API.Models.DTO;

namespace Users.API.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAsync();
        Task<UserDTO> GetByIdAsync(int id);
        Task<UserDTO> CreateAsync(CreateUserDTO createUser);
        Task<bool> UpdateAsync(int id, UpdateUserDTO updateUser);
        Task<bool> DeleteAsync(int id);
    }
}

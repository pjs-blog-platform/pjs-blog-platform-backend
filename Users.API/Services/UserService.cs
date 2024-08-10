using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Users.API.Database;
using Users.API.Models;
using Users.API.Models.DTO;

namespace Users.API.Services
{
    public class UserService : IUserService
    {
        private readonly UsersContext _context;
        private readonly IUserMapper _mapper;

        public UserService(UsersContext context, IUserMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(u => _mapper.Map(u));
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            return _mapper.Map(user);
        }

        public async Task<UserDTO> CreateAsync(CreateUserDTO createUser)
        {
            if (createUser == null)
                throw new ArgumentNullException(nameof(createUser));

            if (string.IsNullOrWhiteSpace(createUser.Username))
                throw new ArgumentException("Username is required", nameof(createUser.Username));

            if (string.IsNullOrWhiteSpace(createUser.Email))
                throw new ArgumentException("Email is required", nameof(createUser.Email));

            if (string.IsNullOrWhiteSpace(createUser.Password))
                throw new ArgumentException("Password is required", nameof(createUser.Password));

            try
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUser.Password);
                var currentTime = DateTime.Now.ToUniversalTime();

                var user = new User()
                {
                    Username = createUser.Username,
                    Email = createUser.Email,
                    PasswordHash = passwordHash,
                    FullName = "",
                    Bio = "",
                    ProfilePictureUrl = "",
                    CreatedAt = currentTime,
                    UpdatedAt = currentTime,
                    IsActive = false
                };

                var userAdded = _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return _mapper.Map(user);
            }
            catch (DbUpdateException ex)
            {
                // Handle unique constraint violation
                if (ex.InnerException != null)
                {
                    var innerException = ex.InnerException;
                    if (innerException != null)
                    {
                        // Check for SQL Server unique constraint violation error codes
                        if (innerException.HResult == -2146233088) // Unique constraint error
                        {
                            var message = innerException.Message;

                            if (message.Contains("Users_Email")) // Replace with actual index name if known
                            {
                                throw new InvalidOperationException("Email already in use", ex);
                            }
                            else if (message.Contains("Users_Username")) // Replace with actual index name if known
                            {
                                throw new InvalidOperationException("Username already in use", ex);
                            }
                        }
                    }
                }

                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDTO updateUser)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
                return false;

            user.FullName = !String.IsNullOrEmpty(updateUser.FullName) ? updateUser.FullName : user.FullName;
            user.Bio = !String.IsNullOrEmpty(updateUser.Bio) ? updateUser.Bio : user.Bio;
            user.ProfilePictureUrl = !String.IsNullOrEmpty(updateUser.ProfilePictureUrl) ? updateUser.ProfilePictureUrl : user.ProfilePictureUrl;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);

            if(user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

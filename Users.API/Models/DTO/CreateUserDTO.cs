using System.ComponentModel.DataAnnotations;

namespace Users.API.Models.DTO
{
    public class CreateUserDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

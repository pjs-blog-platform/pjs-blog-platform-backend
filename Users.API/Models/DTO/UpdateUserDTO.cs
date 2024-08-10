using System.ComponentModel.DataAnnotations;

namespace Users.API.Models.DTO
{
    public class UpdateUserDTO
    {
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}

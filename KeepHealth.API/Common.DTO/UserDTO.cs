using System.ComponentModel.DataAnnotations;

namespace Common.DTO
{
    public class UserDTO
    {
        public long? Id { get; set; }
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
    }
}
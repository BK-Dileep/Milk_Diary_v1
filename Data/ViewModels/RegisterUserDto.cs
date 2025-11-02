using System.ComponentModel.DataAnnotations;

namespace Milk_Diary_v1.Data.ViewModels
{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        public string? PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; } = "";

        public string Address { get; set; } = "";
    }

    public class UsersInfo 
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public string Address { get; set; } = "";
    }

}

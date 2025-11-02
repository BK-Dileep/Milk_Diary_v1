using System.ComponentModel.DataAnnotations;

namespace Milk_Diary_v1.Data.ViewModels
{
    public class LoginUserDto
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }

}

using System.ComponentModel.DataAnnotations;

namespace Milk_Diary_v1.Data.Entity
{
    public class Users
    {
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; }
        [Required, EmailAddress] 
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required] 
        public string Role { get; set; } 
        public string Address { get; set; }

        public bool isactive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }
        public ICollection<MilkOrder> MilkOrders { get; set; }

    }
}

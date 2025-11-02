using Milk_Diary_v1.Data.Entity;

namespace Milk_Diary_v1.Data.ViewModels
{
    public class MilkOrderRequest
    {
        public int UserId { get; set; }
        // Type of milk being ordered: e.g., "Milk", "Special Milk", "Organic Milk"
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Delivery slot: e.g., "Morning", "Evening"
        public string Slot { get; set; }
        public decimal QuantityLiters { get; set; }
        public string? Note { get; set; }
    }

    public class MilkOrderSummaryDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public string UserName { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Slot { get; set; }
        public decimal QuantityLiters { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}

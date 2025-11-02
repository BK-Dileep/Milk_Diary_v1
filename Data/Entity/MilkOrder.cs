namespace Milk_Diary_v1.Data.Entity
{
    public class MilkOrder
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public Users? User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Slot { get; set; } 
        public bool Isactive { get; set; }
        public decimal QuantityLiters { get; set; }
        public decimal PricePerLiter { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifedDate { get; set; }
        public string? Note { get; set; }
    }
}

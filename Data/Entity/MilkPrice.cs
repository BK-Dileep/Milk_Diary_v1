namespace Milk_Diary_v1.Data.Entity
{
    public class MilkPrice
    {
        public int Id { get; set; }
        public decimal PricePerLiter { get; set; }
        public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;
    }
}

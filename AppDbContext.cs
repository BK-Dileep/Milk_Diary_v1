using Microsoft.EntityFrameworkCore;
using Milk_Diary_v1.Data.Entity;
using System.Reflection.Emit;
namespace MilkDiary.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // MilkOrder → Users relationship
        modelBuilder.Entity<MilkOrder>()
            .HasOne(m => m.User)
            .WithMany(u => u.MilkOrders)
            .HasForeignKey(m => m.UserId);

        // Decimal precision for MilkOrder
        modelBuilder.Entity<MilkOrder>(entity =>
        {
            entity.Property(e => e.PricePerLiter).HasPrecision(10, 2);
            entity.Property(e => e.QuantityLiters).HasPrecision(10, 2);
            entity.Property(e => e.TotalAmount).HasPrecision(12, 2);
        });

        // Decimal precision for MilkPrice
        modelBuilder.Entity<MilkPrice>(entity =>
        {
            entity.Property(e => e.PricePerLiter).HasPrecision(10, 2);
        });
    }
    public DbSet<Users> Users => Set<Users>();
    public DbSet<MilkPrice> MilkPrices => Set<MilkPrice>();
    public DbSet<MilkOrder> MilkOrders => Set<MilkOrder>();
}

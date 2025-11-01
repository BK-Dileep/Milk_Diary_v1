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
        modelBuilder.Entity<MilkOrder>()
            .HasOne(m => m.User)                // Navigation property in MilkOrder
            .WithMany(u => u.MilkOrders)       // Collection navigation in Users
            .HasForeignKey(m => m.UserId);     // Foreign key in MilkOrder

    }
    public DbSet<Users> Users => Set<Users>();
    public DbSet<MilkPrice> MilkPrices => Set<MilkPrice>();
    public DbSet<MilkOrder> MilkOrders => Set<MilkOrder>();
}

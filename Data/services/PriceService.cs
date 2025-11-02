using Milk_Diary_v1.Data.Entity;
using MilkDiary.Api.Data;
using Milk_Diary_v1.Helpers.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Milk_Diary_v1.Data.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography.Xml;

namespace MilkDiary.Api.Services
{
    public class PriceService
    {
        private readonly AppDbContext _ctx;

        public PriceService(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<object> GetMilkPrice()
        {
            return await _ctx.MilkPrices.FirstOrDefaultAsync();
        }
        public async Task<bool> UpdateMilkPrice(decimal price)
        {
            var milkprice = new MilkPrice
            {
                PricePerLiter = price,
                EffectiveDate = DateTime.UtcNow
            };
            if (milkprice != null)
            {
                _ctx.MilkPrices.Add(milkprice);
                await _ctx.SaveChangesAsync();
                return true;
            }
            return false;            
        }
    }
}

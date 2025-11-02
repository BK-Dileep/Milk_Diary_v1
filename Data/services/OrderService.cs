using Milk_Diary_v1.Data.Entity;
using MilkDiary.Api.Data;
using Milk_Diary_v1.Helpers.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Milk_Diary_v1.Data.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography.Xml;
using Microsoft.IdentityModel.Tokens;

namespace MilkDiary.Api.Services
{
    public class OrderService
    {
        private readonly AppDbContext _ctx;
        private readonly JwtService _jwt;

        public OrderService(AppDbContext ctx, JwtService jwt)
        {
            _ctx = ctx;
            _jwt = jwt;
        }

        public async Task<List<MilkOrderSummaryDto>> GetOrdersByDateAndSlotAsync(DateTime date, string slot)
        {
            return await _ctx.MilkOrders
        .Where(o => o.StartDate.Date == date.Date && o.Slot == slot)
        .Select(o => new MilkOrderSummaryDto
        {
            Id = o.Id,
            UserId = o.UserId,
            UserName = o.User != null ? o.User.Name : string.Empty,
            PhoneNumber = o.User != null ? o.User.PhoneNumber : string.Empty,
            Address = o.User != null ? o.User.Address : string.Empty,
            Name = o.Name,
            StartDate = o.StartDate,
            EndDate = o.EndDate,
            Slot = o.Slot,
            QuantityLiters = o.QuantityLiters,
            TotalAmount = o.TotalAmount,
            Status = o.Status
        })
        .ToListAsync();
        }

        public async Task<object> GetAllOrderByUserId(int userId)
        {
            return await _ctx.MilkOrders.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<object> MilkOrders(MilkOrderRequest milkOrderRequest)
        {
            var userInfo =  await _ctx.Users.FirstOrDefaultAsync(x => x.Id == milkOrderRequest.UserId);
            var price = await _ctx.MilkPrices.OrderByDescending(x => x.EffectiveDate).Select(x => x.PricePerLiter).FirstOrDefaultAsync();

            if (userInfo != null)
            {
                var milkOrder = new MilkOrder
                {
                    UserId = milkOrderRequest.UserId,
                    Name = milkOrderRequest.Name,
                    QuantityLiters = milkOrderRequest.QuantityLiters,
                    CreatedAt = DateTime.UtcNow,
                    StartDate = milkOrderRequest.StartDate,
                    EndDate = milkOrderRequest.EndDate,
                    Isactive = true,
                    Status = "Pending",
                    Slot = milkOrderRequest.Slot,
                    ModifedDate = null,
                    Note = milkOrderRequest.Note,
                    PricePerLiter = price,
                    TotalAmount = price * milkOrderRequest.QuantityLiters
                };
                _ctx.MilkOrders.Add(milkOrder);
                await _ctx.SaveChangesAsync();
                return new { Success = true, Message = "Order placed successfully" , milkOrder };
            }

            return new { Success = false, Message = "User not found" };
        }

        public async Task<MilkOrder?> UpdateOrder(MilkOrderRequest request)
        {
            var existingOrder = await _ctx.MilkOrders
                .FirstOrDefaultAsync(o => o.UserId == request.UserId && o.StartDate == request.StartDate);

            if (existingOrder == null)
                return null;

            // Update fields
            existingOrder.Name = request.Name;
            existingOrder.StartDate = request.StartDate != default ? request.StartDate : existingOrder.StartDate;
            existingOrder.EndDate = request.EndDate;
            existingOrder.Slot = request.Slot;        
            existingOrder.QuantityLiters = request.QuantityLiters;
            existingOrder.Note = request.Note;
            existingOrder.ModifedDate = DateTime.UtcNow;
            existingOrder.PricePerLiter = await _ctx.MilkPrices
                .OrderByDescending(x => x.EffectiveDate)
                .Select(x => x.PricePerLiter)
                .FirstOrDefaultAsync();
            existingOrder.TotalAmount = existingOrder.QuantityLiters * existingOrder.PricePerLiter;

            await _ctx.SaveChangesAsync();
            return existingOrder;
        }

        public async Task<object> UpdateorderStatus(string orderstatus, List<int> orderIds)
        {
            var orders = await _ctx.MilkOrders
                .Where(o => orderIds.Contains(o.Id))
                .ToListAsync();
            if (orders.Count == 0)
                return false;
            foreach (var order in orders)
            {
                order.Status = orderstatus;
                order.ModifedDate = DateTime.UtcNow;
            }
            _ctx.Update(orders);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelOrder(int orderId)
        {
            var order = await _ctx.MilkOrders
                .FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
                return false;
            order.Isactive = false;
            order.Status = "Cancelled";
            order.ModifedDate = DateTime.UtcNow;
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}

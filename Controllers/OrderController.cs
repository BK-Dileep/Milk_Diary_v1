using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Milk_Diary_v1.Data.ViewModels;
using MilkDiary.Api.Services;

namespace Milk_Diary_v1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("OrdersByDateAndSlot")]
        public async Task<IActionResult> GetOrdersByDateAndSlot(DateTime date, string slot)
        {
            if (string.IsNullOrWhiteSpace(slot))
                return BadRequest("Slot is required.");

            var orders = await _orderService.GetOrdersByDateAndSlotAsync(date, slot);

            if (orders == null || !orders.Any())
                return NotFound("No orders found for the given date and slot.");

            return Ok(orders);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllOrderByUserId(int id)
        {
            var orders = await _orderService.GetAllOrderByUserId(id);
            return Ok(orders);
        }

        [HttpPost("MilkOrders")]
        public async Task<IActionResult> MilkOrders(MilkOrderRequest milkOrderRequest)
        {
            if (milkOrderRequest.UserId == 0)
                return Ok("Id cannot be null");
            var users = await _orderService.MilkOrders(milkOrderRequest);
            return Ok(users);
        }

        [HttpPost("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromBody] MilkOrderRequest milkOrderRequest)
        {
            if (milkOrderRequest.UserId == 0)
                return Ok("Id cannot be null");
            var users = await _orderService.UpdateOrder(milkOrderRequest);
            return Ok(users);
        }

        [HttpPut("{OrderIds}/status")]
        public async Task<IActionResult> UpdateorderStatus(string orderstatus, List<int> orderIds)
        {
            if (orderIds == null || string.IsNullOrEmpty(orderstatus))
                return Ok("OrderId cannot be null");
            var orderstate = await _orderService.UpdateorderStatus(orderstatus, orderIds);
            return Ok(orderstate);
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> CancelOrder(int Id)
        {
            if (Id == 0)
                return Ok("Id cannot be null");
            var users = await _orderService.CancelOrder(Id);
            return Ok(users);
        }



    }
}

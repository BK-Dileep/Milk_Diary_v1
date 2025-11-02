using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkDiary.Api.Services;

namespace Milk_Diary_v1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly PriceService _priceService;

        public PriceController(PriceService priceService)
        {
            _priceService = priceService;
        }

        [HttpGet("GetMilkPrice")]
        public async Task<IActionResult> GetMilkPrice()
        {
            var users = await _priceService.GetMilkPrice();
            return Ok(users);
        }

        [HttpPut("UpdateMilkPrice")]
        public async Task<IActionResult> UpdateMilkPrice(decimal price)
        {
            var ispriceupdate = await _priceService.UpdateMilkPrice(price);
            if (ispriceupdate)
                return Ok(new { message = "Updated Successfully", price, ispriceupdate });

            return Ok("Price not updated");
        }
    }
}

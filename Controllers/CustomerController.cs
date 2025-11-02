using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkDiary.Api.Services;

namespace Milk_Diary_v1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public  CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("GetCustomersList")]
        public async Task<IActionResult> GetNonAdminUsers()
        {
            var users = await _customerService.GetNonAdminUsersAsync();
            return Ok(users);
        }

        [HttpGet("GetCustomersList/{id}")]
        public async Task<IActionResult> GetNonAdminUsersById(int id)
        {
            if (id == 0)
                return Ok("Id cannot be null");
            var users = await _customerService.GetNonAdminUsersById(id);
            return Ok(users);
        }
    }
}

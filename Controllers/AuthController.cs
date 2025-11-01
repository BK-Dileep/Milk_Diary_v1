using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Milk_Diary_v1.Data.Entity;
using Milk_Diary_v1.Helpers.Services;
using MilkDiary.Api.Data;
using System.Security.Cryptography;
using System.Text;
using MilkDiary.Api.Services;

namespace MilkDiary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _ctx;
    private readonly JwtService _jwt;
    private readonly AuthService _auth;

    public AuthController(AuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Users request)
    {
        var result = await _auth.RegisterAsync(request);
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(new { message = result.Message, role = result.Role });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Users request)
    {
        var result = await _auth.LoginAsync(request.Email, request.PasswordHash);
        if (!result.Success)
            return Unauthorized();

        return Ok(new { token = result.Token, role = result.Role, name = result.Name });
    }

}

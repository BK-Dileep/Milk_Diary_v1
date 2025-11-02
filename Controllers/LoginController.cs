using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Milk_Diary_v1.Data.Entity;
using Milk_Diary_v1.Helpers.Services;
using MilkDiary.Api.Data;
using System.Security.Cryptography;
using System.Text;
using MilkDiary.Api.Services;
using Milk_Diary_v1.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MilkDiary.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly LoginService _auth;
    private readonly JwtService _jwt;
    private readonly AppDbContext _ctx;
    public LoginController(LoginService auth, JwtService jwt, AppDbContext ctx)
    {
        _auth = auth;
        _jwt = jwt;
        _ctx = ctx;
    }

    [AllowAnonymous]
    [HttpPost("TokenGenerar")]
    public IActionResult TokenGenerar([FromBody] LoginUserDto testDto)
    {
        var token = _jwt.GenerateToken(testDto);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        if (dto.Email == null)
            return BadRequest("Email already exists");

        var result = await _auth.RegisterAsync(dto);
        return Ok(new { message = "Successfully Registered" });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await _auth.LoginAsync(request);
        if (success == null)
            return Ok(new { message = "Login Failed" });

        return Ok(new { message = "Login Successful" , success});

    }

    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto,int id)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        var success = await _auth.UpdateUser(updateUserDto, id);
        if (!success)
            return Ok(new { message = "User update failed" });  

        return Ok(new { message = "User updated successfully" });
    }

}

using Microsoft.IdentityModel.Tokens;
using Milk_Diary_v1.Data.Entity;
using Milk_Diary_v1.Data.ViewModels;
using MilkDiary.Api.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Milk_Diary_v1.Helpers.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _ctx;

        public JwtService(AppDbContext ctx, IConfiguration config)
        {
            _ctx = ctx;
            _config = config;
        }
        public JwtService(IConfiguration config) => _config = config;

        public string GenerateToken(LoginUserDto user)
        {
            var claimsdata = _ctx.Users.Where(x => x.isactive).FirstOrDefault(u => u.PhoneNumber == user.PhoneNumber || u.Email == user.Email);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, claimsdata.Name.ToString()),
                new Claim(ClaimTypes.Email, claimsdata.Email ?? claimsdata.PhoneNumber),
                new Claim(ClaimTypes.Role, claimsdata.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

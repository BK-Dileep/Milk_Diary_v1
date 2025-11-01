using Milk_Diary_v1.Data.Entity;
using MilkDiary.Api.Data;
using Milk_Diary_v1.Helpers.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MilkDiary.Api.Services
{
    public class AuthService
    {
        private readonly AppDbContext _ctx;
        private readonly JwtService _jwt;
        private readonly List<string> _adminIdentifiers = new() { "admin@milkdiary.com", "9999999999" };

        public AuthService(AppDbContext ctx, JwtService jwt)
        {
            _ctx = ctx;
            _jwt = jwt;
        }

        public async Task<(bool Success, string Message, string Role)> RegisterAsync(Users request)
        {
            if (await _ctx.Users.AnyAsync(u => u.Email == request.Email))
                return (false, "Email already exists", "");

            string role = (_adminIdentifiers.Contains(request.Email) ||
                           (request.PhoneNumber != null && _adminIdentifiers.Contains(request.PhoneNumber)))
                           ? "Admin" : "Customer";

            request.PasswordHash = HashPassword(request.PasswordHash);
            request.Role = role;

            _ctx.Users.Add(request);
            await _ctx.SaveChangesAsync();

            return (true, "Registered successfully", role);
        }

        public async Task<(bool Success, string Token, string Role, string Name)> LoginAsync(string email, string password)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.PasswordHash != HashPassword(password))
                return (false, "", "", "");

            var token = _jwt.GenerateToken(user);
            return (true, token, user.Role, user.Name);
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}

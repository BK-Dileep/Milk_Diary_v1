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
    public class LoginService
    {
        private readonly AppDbContext _ctx;
        private readonly JwtService _jwt;
        private readonly List<string> _adminIdentifiers = new() { "dileep@gmail.com", "9121525189" };

        public LoginService(AppDbContext ctx, JwtService jwt)
        {
            _ctx = ctx;
            _jwt = jwt;
        }

        public async Task<(bool Success, string Message, string Role)> RegisterAsync(RegisterUserDto request)
        {
            if (await _ctx.Users.AnyAsync(u => u.Email == request.Email || u.PhoneNumber == request.PhoneNumber))
                return (false, "Email or PhoneNumber already exists", "");

            string role = (_adminIdentifiers.Contains(request.Email) ||
                           (request.PhoneNumber != null && _adminIdentifiers.Contains(request.PhoneNumber)))
                           ? "Admin" : "Customer";

            var user = new Users
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = HashPassword(request.Password),
                Role = role,
                isactive = true,
                Address = request.Address,
                CreatedAt = DateTime.UtcNow,
                ModifiedDate = null
            };

            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();

            return (true, "Registered successfully", role);
        }

        public async Task<object> LoginAsync(LoginUserDto loginUserDto)
        {
            var user = await _ctx.Users.Where( x => x.isactive).FirstOrDefaultAsync(u =>
                (!string.IsNullOrEmpty(loginUserDto.Email) && u.Email == loginUserDto.Email) ||
                (!string.IsNullOrEmpty(loginUserDto.PhoneNumber) && u.PhoneNumber == loginUserDto.PhoneNumber));

            if (user == null || user.PasswordHash != HashPassword(loginUserDto.Password))
                return false;
            //var token = _jwt.GenerateToken(loginUserDto);
            // Token generation skipped intentionally
            return user;
        }

        public async Task<bool> UpdateUser(UpdateUserDto updateUserDto, int id)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return false;
            user.Name = updateUserDto.Name ?? user.Name;
            user.Address = updateUserDto.Address ?? user.Address;
            user.PhoneNumber = updateUserDto.PhoneNumber ?? user.PhoneNumber;
            user.PasswordHash = !string.IsNullOrEmpty(updateUserDto.Password) ? HashPassword(updateUserDto.Password) : user.PasswordHash;
            user.ModifiedDate = DateTime.UtcNow;
            _ctx.Users.Update(user);
            await _ctx.SaveChangesAsync();

            return true;
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}

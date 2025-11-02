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
    public class CustomerService
    {
        private readonly AppDbContext _ctx;
        private readonly JwtService _jwt;

        public CustomerService(AppDbContext ctx, JwtService jwt)
        {
            _ctx = ctx;
            _jwt = jwt;
        }

        public async Task<List<UsersInfo>> GetNonAdminUsersAsync()
        {
            return await _ctx.Users
                .Where(u => u.Role != "Admin").Select(x => new UsersInfo { 
                    Name = x.Name,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email
                }).ToListAsync();
        }

        public async Task<UsersInfo> GetNonAdminUsersById(int id)
        {
            var userInfo =  await _ctx.Users
                .Where(u => u.Role != "Admin" && u.Id == id).Select(x => new UsersInfo
                {
                    Name = x.Name,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email
                }).FirstOrDefaultAsync();
            return userInfo;
        }

    }
}

using Microsoft.EntityFrameworkCore;
using MilkDiary.Api.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Milk_Diary_v1.Helpers.Services;
using MilkDiary.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<JwtService>();
builder.Services.AddControllers();
builder.Services.AddCors(p => p.AddPolicy("cors",
    b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthService>();
var app = builder.Build();
app.UseCors("cors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

using backend.Models;  // For ApplicationUser
using Microsoft.AspNetCore.Identity;  // For IdentityRole

using backend.Data;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging()
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add application services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketService, TicketService>();

// Add JWT Authentication
var secretKey = builder.Configuration["Jwt:SecretKey"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("Jwt:SecretKey missing in configuration!");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],   // Accessing Jwt:Issuer from config
        ValidAudience = builder.Configuration["Jwt:Audience"], // Accessing Jwt:Audience from config
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey) // ✅ safe
        ) // Accessing Jwt:SecretKey from config
    };
});

builder.Services.AddControllers();

// Configure CORS (if needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
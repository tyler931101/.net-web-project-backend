using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResult> RegisterAsync(ApplicationUser user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email!);
            if (existingUser != null)
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Message = "❌ Email already taken"
                };
            }

            var result = await _userManager.CreateAsync(user, user.Password!);
            if (result.Succeeded)
            {
                return new AuthResult
                {
                    IsSuccess = true,
                    Message = "✅ Registered successfully",
                    UserId = user.Id
                };
            }

            return new AuthResult
            {
                IsSuccess = false,
                Message = "❌ Registration failed",
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<AuthResult> LoginAsync(ApplicationUser user)
        {
            var dbUser = await _userManager.FindByEmailAsync(user.Email!);
            if (dbUser == null || !await _userManager.CheckPasswordAsync(dbUser, user.Password!))
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Message = "❌ Invalid credentials"
                };
            }

            var token = GenerateJwtToken(dbUser);
            return new AuthResult
            {
                IsSuccess = true,
                Message = "✅ Login successful",
                Token = token,
                UserId = dbUser.Id
            };
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? "")
            };

            var secretKey = _configuration["Jwt:SecretKey"] 
                ?? throw new InvalidOperationException("Jwt:SecretKey missing");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
using System.Reflection.Metadata;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Logging;

namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;  // Initialize the logger
        }

        public async Task<AuthResult> RegisterAsync(RegisterModel model)
        {

            var existingUser = await _userManager.FindByNameAsync(model.Email);

            if (existingUser != null)
            {
                return new AuthResult{
                    IsSuccess = false,
                    Message = "❌ Registration failed",
                    Errors = new List<string> { "Username or email is already taken." }
                };
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return new AuthResult
                {
                    IsSuccess = true,
                    Message = "✅ Registration successful!",
                    UserId = user.Id
                };
            }

            // Handle multiple errors
            return new AuthResult
            {
                IsSuccess = false,
                Message = "❌ Registration failed",
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<AuthResult> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                _logger.LogError($"Invalid login attempt for email: {model.Email}");
                return new AuthResult(false, string.Empty);
            }

            var token = GenerateJwtToken(user);
            _logger.LogInformation($"Login successful for email: {model.Email}");

            return new AuthResult(true, token, user.Id);
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? ""),  // Use null-coalescing operator to handle null values
                new Claim(ClaimTypes.Name, user.Email ?? "")  // Same here
            };

            var secretKey = _configuration["Jwt:SecretKey"]; // Set a default value here
            if (string.IsNullOrEmpty(secretKey))
                throw new InvalidOperationException("Jwt:SecretKey is missing in appsettings.json");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "",
                audience: _configuration["Jwt:Audience"] ?? "",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
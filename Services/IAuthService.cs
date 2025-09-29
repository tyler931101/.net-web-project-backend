using backend.Models;

namespace backend.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterModel model);  // Only one definition
        Task<AuthResult> LoginAsync(LoginModel model);  // Only one definition
    }
}
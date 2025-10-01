using backend.Models;

namespace backend.Services
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(ApplicationUser user);
        Task<AuthResult> LoginAsync(ApplicationUser user);
    }
}
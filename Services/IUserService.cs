using backend.Models;

namespace backend.Services
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);
    }
}
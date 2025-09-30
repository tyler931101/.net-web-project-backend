using backend.Models;

namespace backend.Services
{
    public interface IUserService
    {
        Task<List<UserModel>> GetUsersAsync();
        Task<UserModel?> GetUserByIdAsync(string id);
    }
}
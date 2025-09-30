using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserModel>> GetUsersAsync()
        {
            try
            {
                return await _context.Users
                    .Select(u => new UserModel
                    {
                        Id = u.Id,
                        Name = u.UserName,
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUsersAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<UserModel?> GetUserByIdAsync(string id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    Name = u.UserName!,
                })
                .FirstOrDefaultAsync();
        }
    }
}
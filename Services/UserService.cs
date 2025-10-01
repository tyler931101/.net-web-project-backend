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

        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            try
            {
                return await _context.Users
                    .Select(u => new ApplicationUser
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUsersAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new ApplicationUser
                {
                    Id = u.Id,
                    UserName = u.UserName!,
                })
                .FirstOrDefaultAsync();
        }
    }
}
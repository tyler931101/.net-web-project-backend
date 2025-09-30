using backend.Models;

namespace backend.Services
{
    public interface ITicketService
    {
        Task<List<TicketModel>> GetTicketsAsync();
        Task<TicketModel?> GetTicketByIdAsync(string id);
        Task<TicketModel> CreateTicketAsync(TicketModel ticket);
        Task<bool> UpdateTicketAsync(string id, TicketModel ticket);
        Task<bool> DeleteTicketAsync(string id);
    }
}
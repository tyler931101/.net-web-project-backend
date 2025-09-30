using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TicketModel>> GetTicketsAsync()
            => await _context.Tickets.ToListAsync();

        public async Task<TicketModel?> GetTicketByIdAsync(string id)
            => await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);

        public async Task<TicketModel> CreateTicketAsync(TicketModel ticket)
        {
            ticket.Id = Guid.NewGuid().ToString();
            _context.Tickets.Add(ticket);

            var rows = await _context.SaveChangesAsync();

            Console.WriteLine($"[DEBUG] Rows saved: {rows}");
            Console.WriteLine($"[DEBUG] DB path: {_context.Database.GetDbConnection().DataSource}");

            var count = await _context.Tickets.CountAsync();
            Console.WriteLine($"[DEBUG] Total tickets in DB: {count}");

            return ticket;
        }

        public async Task<bool> UpdateTicketAsync(string id, TicketModel ticket)
        {
            if (id != ticket.Id) return false;

            _context.Entry(ticket).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return await _context.Tickets.AnyAsync(e => e.Id == id);
            }
        }

        public async Task<bool> DeleteTicketAsync(string id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null) return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
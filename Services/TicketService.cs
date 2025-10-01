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
        {
            var tickets = await _context.Tickets
                .Include(t => t.Performer)
                .ToListAsync();

            // 🚀 now safely map PerformerName in memory
            foreach (var t in tickets)
            {
                t.PerformerName = t.Performer?.UserName ?? string.Empty;
                t.Performer = null; // optional: prevent IdentityUser serialization
            }

            return tickets;
        }

        public async Task<TicketModel?> GetTicketByIdAsync(string id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Performer)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket != null)
            {
                ticket.PerformerName = ticket.Performer?.UserName ?? string.Empty;
                ticket.Performer = null; // optional
            }

            return ticket;
        }
        public async Task<TicketModel> CreateTicketAsync(TicketModel ticket)
        {
            ticket.Id = Guid.NewGuid().ToString();
            _context.Tickets.Add(ticket);

            await _context.SaveChangesAsync();
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
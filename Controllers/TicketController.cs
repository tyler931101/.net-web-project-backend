using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketModel>>> GetAll()
            => Ok(await _ticketService.GetTicketsAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketModel>> GetById(string id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<TicketModel>> Create(TicketModel ticket)
        {
            ticket.Id = Guid.NewGuid().ToString();
            var created = await _ticketService.CreateTicketAsync(ticket);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, TicketModel ticket)
        {
            var success = await _ticketService.UpdateTicketAsync(id, ticket);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _ticketService.DeleteTicketAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
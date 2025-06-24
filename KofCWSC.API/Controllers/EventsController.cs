using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Data;
using KofCWSC.API.Models;

namespace KofCWSC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public EventsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            return await _context.Events.OrderBy(e => e.StartDate).ToListAsync();
        }

        // GET: api/Events/range?start=2025-01-01&end=2025-12-31
        [HttpGet("range")]
        public IActionResult GetEventsInRange([FromQuery] string start, [FromQuery] string end)
        {
            if (!DateTime.TryParse(start, out var startDate))
                return BadRequest("Invalid start date.");

            if (!DateTime.TryParse(end, out var endDate))
                endDate = DateTime.MaxValue;

            var events = _context.Events
                .Where(e => e.StartDate >= startDate && e.EndDate <= endDate)
                .OrderBy(e => e.StartDate)
                .ToList();

            return Ok(events);
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEventById(int id)
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null)
                return NotFound();

            return eventItem;
        }

        // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent([FromBody] Event newEvent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetEventById), new { id = newEvent.EventId }, newEvent);
            }
            catch (Exception ex)
            {
                // Optional: Log error using your own utility if available
                // Utils.Helper.FormatLogEntry(this, ex);
                return  StatusCode(500, "An error occurred while saving the event.");
            }
        }

        // PUT: api/Events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event updatedEvent)
        {
            if (id != updatedEvent.EventId)
                return BadRequest("ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(updatedEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Events.Any(e => e.EventId == id))
                    return NotFound();

                throw;
            }
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
                return NotFound();

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}


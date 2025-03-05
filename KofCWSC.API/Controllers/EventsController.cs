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
    [Route("")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public EventsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: /Events
        [HttpGet("Events")]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            return await _context.Events.OrderBy(e => e.StartDate).ToListAsync();
        }

        // GET: /Events/{start}/{end}
        [HttpGet("Events/{start}/{end}")]
        public IActionResult GetCalendarEvents(string start, string end)
        {
            // Ensure date formats are correct
            DateTime startDate = DateTime.Parse(start.Replace("%2F", "/"));
            DateTime endDate = DateTime.TryParse(end.Replace("%2F", "/"), out var parsedEndDate) ? parsedEndDate : DateTime.MaxValue;

            List<Event> events = _context.Events
                .Where(e => e.StartDate >= startDate && e.EndDate <= endDate)
                .ToList();

            return Ok(events);
        }

        // GET: /Event/{id}
        [HttpGet("Event/{id}")]
        public async Task<IActionResult> GetEventDetails(int id)
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.EventId == id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return Ok(eventItem);
        }

        // POST: /Event
        [HttpPost("Event")]
        public async Task<IActionResult> CreateEvent([FromBody] Event newEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEventDetails), new { id = newEvent.EventId }, newEvent);
        }

        // PUT: /Event/{id}
        [HttpPut("Event/{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event updatedEvent)
        {
            if (id != updatedEvent.EventId)
            {
                return BadRequest();
            }

            _context.Entry(updatedEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Events.Any(e => e.EventId == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: /Event/{id}
        [HttpDelete("Event/{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}


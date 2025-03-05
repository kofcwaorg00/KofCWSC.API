using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KofCWSC.API.Models;
using KofCWSC.API.Data;

namespace KofCWSC.API.Controllers
{
    [Route("")]
    [ApiController]
    public class CalendarsController : ControllerBase // ✅ Changed to ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public CalendarsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: /Calendars
        [HttpGet("Calendars")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents() // ✅ Changed Events → Event
        {
            return await _context.Events
                .OrderBy(e => e.StartDate)
                .ToListAsync();
        }

        // GET: /Calendar/{id}
        [HttpGet("Calendar/{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id) // ✅ Changed Events → Event
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.EventId == id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return eventItem;
        }

        // POST: /Calendar
        [HttpPost("Calendar")]
        public async Task<ActionResult<Event>> CreateEvent(Event eventItem) // ✅ Changed Events → Event
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = eventItem.EventId }, eventItem);
        }

        // PUT: /Calendar/{id}
        [HttpPut("Calendar/{id}")]
        public async Task<IActionResult> EditEvent(int id, Event eventItem) // ✅ Changed Events → Event
        {
            if (id != eventItem.EventId)
            {
                return BadRequest();
            }

            _context.Entry(eventItem).State = EntityState.Modified;

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
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE: /Calendar/{id}
        [HttpDelete("Calendar/{id}")]
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
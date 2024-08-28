using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Data;
using KofCWSC.API.Models;
using System.Text.Json;

namespace KofCWSC.API.Controllers
{
    [Route("")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public EventsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: TblSysTrxEvents
        [HttpGet("Events")]
        public async Task<ActionResult<IEnumerable<TblSysTrxEvents>>> Index()
        {
            return await _context.TblSysTrxEvents.ToListAsync();
        }

        [HttpGet("Events/{end}/{start}")]
        public IActionResult GetCalendarEvents(string start, string end)
        {
            //***********************************************************************
            // 8/28/2024 Tim Philomeno 
            // the issue is that the end date is not requred in the database and can
            // come in null so for now I am only checking start date.  We will need
            // to define how we want these date ranges to work when we begin using
            // the calendar
            //***********************************************************************
            var myStart = start.Replace("%2F", "/");
            var myEnd = "12/31/3000"; // end.Replace("%2F", "/");
            DateTime startdate = DateTime.Parse(myStart);
            DateTime enddate = DateTime.Parse(myEnd);
            List<TblSysTrxEvents> events = _context
                .TblSysTrxEvents
                .Where(l => l.Begin >= startdate) // && l.End <= enddate)
                .ToList();
            
            return Json(events);
        }

        // GET: TblSysTrxEvents/Details/5
        [HttpGet("Event/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblSysTrxEvent = await _context.TblSysTrxEvents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblSysTrxEvent == null)
            {
                return NotFound();
            }

            return Json(tblSysTrxEvent);
        }


        // POST: TblSysTrxEvents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Event")]
        ////[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] TblSysTrxEvents tblSysTrxEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblSysTrxEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return Json(tblSysTrxEvent);
        }

        [HttpPut("Event/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] TblSysTrxEvents tblSysTrxEvent)
        {
            if (id != tblSysTrxEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblSysTrxEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblSysTrxEventExists(tblSysTrxEvent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return Json(tblSysTrxEvent);
        }


        // POST: TblSysTrxEvents/Delete/5
        [HttpDelete("Event/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblSysTrxEvent = await _context.TblSysTrxEvents.FindAsync(id);
            if (tblSysTrxEvent != null)
            {
                _context.TblSysTrxEvents.Remove(tblSysTrxEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblSysTrxEventExists(int id)
        {
            return _context.TblSysTrxEvents.Any(e => e.Id == id);
        }
    }
}

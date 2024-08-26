using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Data;
using KofCWSC.API.Models;

namespace KofCWSC.API.Controllers
{
    [Route("")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public OfficesController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/Offices
        [HttpGet("Offices")]
        public async Task<ActionResult<IEnumerable<TblValOffice>>> GetOffices()
        {
            return await _context.TblValOffices.ToListAsync();
        }

        // GET: api/Offices/5
        [HttpGet("Office/{id}")]
        public async Task<ActionResult<TblValOffice>> GetOffice(int id)
        {
            var tblValOffice = await _context.TblValOffices.FindAsync(id);

            if (tblValOffice == null)
            {
                return NotFound();
            }

            return tblValOffice;
        }

        // POST: api/Offices
        [HttpPost("Office")]
        public async Task<ActionResult<TblValOffice>> CreateOffice(TblValOffice tblValOffice)
        {
            _context.TblValOffices.Add(tblValOffice);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOffice), new { id = tblValOffice.OfficeId }, tblValOffice);
        }

        // PUT: api/Offices/5
        [HttpPut("Office/{id}")]
        public async Task<IActionResult> UpdateOffice(int id, TblValOffice tblValOffice)
        {
            if (id != tblValOffice.OfficeId)
            {
                return BadRequest();
            }

            _context.Entry(tblValOffice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfficeExists(id))
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

        // DELETE: api/Offices/5
        [HttpDelete("Office/{id}")]
        public async Task<IActionResult> DeleteOffice(int id)
        {
            var tblValOffice = await _context.TblValOffices.FindAsync(id);
            if (tblValOffice == null)
            {
                return NotFound();
            }

            _context.TblValOffices.Remove(tblValOffice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OfficeExists(int id)
        {
            return _context.TblValOffices.Any(e => e.OfficeId == id);
        }
    }
}

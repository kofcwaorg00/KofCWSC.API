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
    public class AssembliesController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public AssembliesController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/TblValAssys
        [HttpGet("Assys")]
        public async Task<ActionResult<IEnumerable<TblValAssy>>> GetTblValAssys()
        {
            return await _context.TblValAssy
                .OrderBy(x => x.ANumber)
                .ToListAsync();
        }

        // GET: api/TblValAssys/5
        [HttpGet("Assy/{id}")]
        public async Task<ActionResult<TblValAssy>> GetTblValAssy(int id)
        {
            var tblValAssy = await _context.TblValAssy.FindAsync(id);

            if (tblValAssy == null)
            {
                return NotFound();
            }

            return tblValAssy;
        }

        // POST: api/TblValAssys
        [HttpPost("Assy")]
        public async Task<ActionResult<TblValAssy>> PostTblValAssy([FromBody]TblValAssy tblValAssy)
        {
            _context.TblValAssy.Add(tblValAssy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTblValAssy), new { id = tblValAssy.ANumber }, tblValAssy);
        }

        // PUT: api/TblValAssys/5
        [HttpPut("Assy/{id}")]
        public async Task<IActionResult> PutTblValAssy(int id, TblValAssy tblValAssy)
        {
            if (id != tblValAssy.ANumber)
            {
                return BadRequest();
            }

            _context.Entry(tblValAssy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblValAssyExists(id))
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

        // DELETE: api/TblValAssys/5
        [HttpDelete("Assy/{id}")]
        public async Task<IActionResult> DeleteTblValAssy(int id)
        {
            var tblValAssy = await _context.TblValAssy.FindAsync(id);
            if (tblValAssy == null)
            {
                return NotFound();
            }

            _context.TblValAssy.Remove(tblValAssy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblValAssyExists(int id)
        {
            return _context.TblValAssy.Any(e => e.ANumber == id);
        }
    }
}

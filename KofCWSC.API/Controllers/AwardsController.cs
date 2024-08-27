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
    public class AwardsController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public AwardsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/MasAwards
        [HttpGet("Awards")]
        public async Task<ActionResult<IEnumerable<TblMasAward>>> GetAwards()
        {
            return await _context.TblMasAwards.ToListAsync();
        }

        // GET: api/MasAwards/5
        [HttpGet("Award/{id}")]
        public async Task<ActionResult<TblMasAward>> GetAward(int id)
        {
            var award = await _context.TblMasAwards.FindAsync(id);

            if (award == null)
            {
                return NotFound();
            }

            return award;
        }

        // POST: api/MasAwards
        [HttpPost("Award")]
        public async Task<ActionResult<TblMasAward>> PostAward(TblMasAward award)
        {
            _context.TblMasAwards.Add(award);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAward", new { id = award.Id }, award);
        }

        // PUT: api/MasAwards/5
        [HttpPut("Award/{id}")]
        public async Task<IActionResult> PutAward(int id, TblMasAward award)
        {
            if (id != award.Id)
            {
                return BadRequest();
            }

            _context.Entry(award).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AwardExists(id))
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

        // DELETE: api/MasAwards/5
        [HttpDelete("Award/{id}")]
        public async Task<IActionResult> DeleteAward(int id)
        {
            var award = await _context.TblMasAwards.FindAsync(id);
            if (award == null)
            {
                return NotFound();
            }

            _context.TblMasAwards.Remove(award);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AwardExists(int id)
        {
            return _context.TblMasAwards.Any(e => e.Id == id);
        }
    }
}

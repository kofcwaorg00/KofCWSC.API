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
    [Route("[controller]")]
    [ApiController]
    public class TblMasPSOsController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public TblMasPSOsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/TblMasPSOs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblMasPso>>> GetTblMasPsos()
        {
            return await _context.TblMasPsos.ToListAsync();
        }

        // GET: api/TblMasPSOs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblMasPso>> GetTblMasPso(int id)
        {
            var tblMasPso = await _context.TblMasPsos.FindAsync(id);

            if (tblMasPso == null)
            {
                return NotFound();
            }

            return tblMasPso;
        }
        // POST: api/TblMasPSOs
        [HttpPost]
        public async Task<ActionResult<TblMasPso>> PostTblMasPso(TblMasPso tblMasPso)
        {
            _context.TblMasPsos.Add(tblMasPso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTblMasPso), new { id = tblMasPso.Id }, tblMasPso);
        }

        // PUT: api/TblMasPSOs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblMasPso(int id, TblMasPso tblMasPso)
        {
            if (id != tblMasPso.Id)
            {
                return BadRequest();
            }

            _context.Entry(tblMasPso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblMasPsoExists(id))
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

        // DELETE: api/TblMasPSOs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblMasPso(int id)
        {
            var tblMasPso = await _context.TblMasPsos.FindAsync(id);
            if (tblMasPso == null)
            {
                return NotFound();
            }

            _context.TblMasPsos.Remove(tblMasPso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblMasPsoExists(int id)
        {
            return _context.TblMasPsos.Any(e => e.Id == id);
        }
    }

}


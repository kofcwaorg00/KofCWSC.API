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
    public class AoisController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public AoisController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: TblWebTrxAois
        [HttpGet("Aois")]
        public async Task<ActionResult<IEnumerable<TblWebTrxAoi>>> GetAOIs()
        {
            return await _context.TblWebTrxAois
                .OrderByDescending(o => o.PostedDate)
                .ToListAsync();
        }

        // GET: TblWebTrxAois/Details/5
        [HttpGet("Aoi/{id}")]
        public async Task<ActionResult<TblWebTrxAoi>> GetAOI(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblWebTrxAoi = await _context.TblWebTrxAois
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblWebTrxAoi == null)
            {
                return NotFound();
            }

            return tblWebTrxAoi;
        }

        // POST: TblWebTrxAois/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Aoi")]
        public async Task<ActionResult<TblWebTrxAoi>> CreateAOI(TblWebTrxAoi tblWebTrxAoi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblWebTrxAoi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return tblWebTrxAoi;
        }

        // POST: TblWebTrxAois/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("Aoi/{id}")]
        public async Task<IActionResult> EditAOI(int id, TblWebTrxAoi tblWebTrxAoi)
        {
            if (id != tblWebTrxAoi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblWebTrxAoi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblWebTrxAoiExists(tblWebTrxAoi.Id))
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
            return View(tblWebTrxAoi);
        }

        // POST: TblWebTrxAois/Delete/5
        [HttpDelete("Aoi/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblWebTrxAoi = await _context.TblWebTrxAois.FindAsync(id);
            if (tblWebTrxAoi != null)
            {
                _context.TblWebTrxAois.Remove(tblWebTrxAoi);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool TblWebTrxAoiExists(int id)
        {
            return _context.TblWebTrxAois.Any(e => e.Id == id);
        }
    }
}

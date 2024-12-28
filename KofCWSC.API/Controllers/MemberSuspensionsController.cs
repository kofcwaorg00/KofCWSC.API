using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Serilog;


namespace KofCWSC.API.Controllers
{
    public class MemberSuspensionsController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public MemberSuspensionsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: MemberSuspensions
        [HttpGet("Suspensions")]
        public async Task<ActionResult<IEnumerable<MemberSuspensionVM>>> Index()
        {
            return await _context.Database.SqlQuery<MemberSuspensionVM>($"uspSYS_GetSuspendedMembers").ToListAsync();
        }

        // GET: MemberSuspensions/Details/5
        [HttpGet("Suspensions/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            var memberSuspensions = await _context.TblSysMasMemberSuspensions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (memberSuspensions == null)
            {
                return NotFound();
            }
            return Ok(memberSuspensions);

        }

        // POST: MemberSuspensions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Suspensions/CreateFrom")]
        public async Task<IActionResult> CreateFrom([FromBody] MemberSuspension memberSuspension)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(memberSuspension);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return CreatedAtAction(nameof(CreateFrom), new { id = memberSuspension.Id }, memberSuspension);

            }
            catch (Exception ex)
            {
                Log.Error(Utils.Helper.FormatLogEntry(this, ex));
                return BadRequest(ex.Message);
            }
        }


        // POST: MemberSuspensions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Suspensions")]
        public async Task<IActionResult> Create([FromBody] MemberSuspension memberSuspension)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(memberSuspension);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return CreatedAtAction(nameof(Create), new { id = memberSuspension.Id }, memberSuspension);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        // POST: MemberSuspensions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("Suspensions/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[FromBody] MemberSuspension memberSuspension)
        {
            if (id != memberSuspension.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberSuspension);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberSuspensionExists(memberSuspension.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok();
            }
            return Ok();
        }


        // POST: MemberSuspensions/Delete/5
        [HttpDelete("Suspensions/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memberSuspension = await _context.TblSysMasMemberSuspensions.FindAsync(id);
            if (memberSuspension != null)
            {
                _context.TblSysMasMemberSuspensions.Remove(memberSuspension);
                
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool MemberSuspensionExists(int id)
        {
            return _context.TblSysMasMemberSuspensions.Any(e => e.Id == id);
        }
    }
}

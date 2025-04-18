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
    public class LogCorrMemberOfficesController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public LogCorrMemberOfficesController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        ////////// GET: LogCorrMemberOffices
        ////////[HttpGet("LogCorrMemberOffices")]
        ////////public async Task<ActionResult<IEnumerable<LogCorrMemberOffice>>> GetCorrMemberOffices()
        ////////{
        ////////    return await _context.TblLogCorrMemberOffices.ToListAsync();
        ////////}

        // GET: api/SP/GetDDs
        [HttpGet("LogCorrMemberOffices")]
        public async Task<ActionResult<LogCorrMemberOfficeVM>> GetCorrMemberOffices()
        {
            try
            {
                var results = await _context.Database.SqlQuery<LogCorrMemberOfficeVM>($"EXECUTE uspLOG_GetCMOLog").ToListAsync();
               
                    return Ok(results);
               
                
            }
            catch (Exception ex)
            {
                Utils.Helper.FormatLogEntry(this, ex);
                return BadRequest(ex.Message);
            }
            
        }

       

        // GET: api/SP/GetDDs
        [HttpGet("ProcLogCorrMemberOffices/{id}")]
        public int ToggleProcessFlag(int id)
        {
            try
            {
                return _context.Database.ExecuteSql($"EXECUTE [uspLOG_UpdateProcessFlag] {id}");

            }
            catch (Exception ex)
            {
                Utils.Helper.FormatLogEntry(this, ex);
                return 0;
            }

        }

        // POST: LogCorrMemberOffices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        ////////[HttpPost("LogCorrMemberOffices")]
        ////////public async Task<IActionResult> Create([FromBody] LogCorrMemberOffice tblLogCorrMemberOffice)
        ////////{
        ////////    if (ModelState.IsValid)
        ////////    {
        ////////        _context.Add(tblLogCorrMemberOffice);
        ////////        await _context.SaveChangesAsync();
        ////////        return RedirectToAction(nameof(Index));
        ////////    }
        ////////    return Ok();
        ////////}


        // POST: LogCorrMemberOffices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,ChangeType,ChangeDate,MemberId,OfficeId,PrimaryOffice,Year,District,Council,Assembly,Processed")] TblLogCorrMemberOffice tblLogCorrMemberOffice)
        //{
        //    if (id != tblLogCorrMemberOffice.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(tblLogCorrMemberOffice);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TblLogCorrMemberOfficeExists(tblLogCorrMemberOffice.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(tblLogCorrMemberOffice);
        //}

        // POST: LogCorrMemberOffices/Delete/5
        //[HttpPost, ActionName("Delete")]
        [HttpDelete("LogCorrMemberOffices/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblLogCorrMemberOffice = await _context.TblLogCorrMemberOffices.FindAsync(id);
            if (tblLogCorrMemberOffice != null)
            {
                _context.TblLogCorrMemberOffices.Remove(tblLogCorrMemberOffice);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        //////////private bool TblLogCorrMemberOfficeExists(int id)
        //////////{
        //////////    return _context.TblLogCorrMemberOffices.Any(e => e.Id == id);
        //////////}
    }
}

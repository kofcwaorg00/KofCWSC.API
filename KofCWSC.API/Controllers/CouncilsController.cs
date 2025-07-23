using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KofCWSC.API.Controllers
{
    [Route("")]
    [ApiController]
    public class CouncilsController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public CouncilsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/TblValCouncils
        [HttpGet("Councils")]
        public async Task<ActionResult<IEnumerable<TblValCouncil>>> GetTblValCouncils()
        {
            return await _context.TblValCouncils
                .Where(x => x.CNumber > 0)
                .OrderBy(x => x.CNumber)
                .ToListAsync();
        }

        // GET: api/TblValCouncils/5
        [HttpGet("FSEditCouncil/{id}")]
        public async Task<ActionResult<TblValCouncilFSEdit>> GetTblValCouncilFSEdit(int id)
        {
            try
            {
                var tblValCouncil = _context.TblValCouncilsFSEdit.FromSql($"EXEC uspCVN_GetFSEdit {id}").AsEnumerable().FirstOrDefault();
                if (tblValCouncil == null)
                {
                    return Empty;
                }
                return tblValCouncil;

            }
            catch (Exception ex)
            {
                Log.Error(Utils.Helper.FormatLogEntry(this, ex));
                return Empty;
            }
        }

        // GET: api/TblValCouncils/5
        [HttpGet("Council/{id}")]
        public async Task<ActionResult<TblValCouncil>> GetTblValCouncil(int id)
        {
            var tblValCouncil = await _context.TblValCouncils
                .FirstOrDefaultAsync(m => m.CNumber == id);
            if (tblValCouncil == null)
            {
                return NotFound();
            }
            return Ok(tblValCouncil);
        }

        // POST: api/TblValCouncils
        [HttpPost("Council")]
        public async Task<ActionResult<TblValCouncil>> CreateTblValCouncil(TblValCouncil tblValCouncil)
        {
            _context.TblValCouncils.Add(tblValCouncil);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTblValCouncil), new { id = tblValCouncil.CNumber }, tblValCouncil);
        }

        [HttpPut("Council/MPD/{id}")]
        public async Task<ActionResult<TblValCouncil>> UpdateTblValCouncilMPD(int id, [FromBody] TblValCouncilMPD tblValCouncil)
        {
            if (id != tblValCouncil.CNumber)
            {
                // not sure if this would ever happen but we need to communicate back
                // to the calling process what happened
                return BadRequest("Council numbers mismatch");
            }
            var council = new TblValCouncil { CNumber = id };
            _context.Attach(council);
            council.SeatedDelegateDay1D1 = tblValCouncil.SeatedDelegateDay1D1;
            _context.Entry(council).Property(e => e.SeatedDelegateDay1D1).IsModified = true;

            council.SeatedDelegateDay1D2 = tblValCouncil.SeatedDelegateDay1D2;
            _context.Entry(council).Property(e => e.SeatedDelegateDay1D2).IsModified = true;

            council.SeatedDelegateDay2D1 = tblValCouncil.SeatedDelegateDay2D1;
            _context.Entry(council).Property(e => e.SeatedDelegateDay2D1).IsModified = true;

            council.SeatedDelegateDay2D2 = tblValCouncil.SeatedDelegateDay2D2;
            _context.Entry(council).Property(e => e.SeatedDelegateDay2D2).IsModified = true;

            council.SeatedDelegateDay3D1 = tblValCouncil.SeatedDelegateDay3D1;
            _context.Entry(council).Property(e => e.SeatedDelegateDay3D1).IsModified = true;

            council.SeatedDelegateDay3D2 = tblValCouncil.SeatedDelegateDay3D2;
            _context.Entry(council).Property(e => e.SeatedDelegateDay3D2).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblValCouncilExists(id))
                {
                    return BadRequest("Concurrency Issue");
                }
                else
                {
                    return BadRequest("Unknown");
                }
            }

            return Ok(tblValCouncil);
        }


        //PUT: Only Update the address and meeting info
        [HttpPut("Council/FSEdit/{id}")]
        public async Task<ActionResult<TblValCouncil>> UpdateTblValCouncilFSEdit(int id, [FromBody] TblValCouncilFSEdit tblValCouncil)
        {
            if (id != tblValCouncil.CNumber)
            {
                // not sure if this would ever happen but we need to communicate back
                // to the calling process what happened
                return BadRequest("Council numbers mismatch");
            }
            var council = new TblValCouncil { CNumber = id };
            _context.Attach(council);
            // Physical address
            council.PhyAddress = tblValCouncil.PhyAddress;
            _context.Entry(council).Property(e => e.PhyAddress).IsModified = true;

            council.PhyCity = tblValCouncil.PhyCity;
            _context.Entry(council).Property(e => e.PhyCity).IsModified = true;

            council.PhyState = tblValCouncil.PhyState;
            _context.Entry(council).Property(e => e.PhyState).IsModified = true;

            council.PhyPostalCode = tblValCouncil.PhyPostalCode;
            _context.Entry(council).Property(e => e.PhyPostalCode).IsModified = true;
            // Meeting Address
            council.MeetAddress = tblValCouncil.MeetAddress;
            _context.Entry(council).Property(e => e.MeetAddress).IsModified = true;

            council.MeetCity = tblValCouncil.MeetCity;
            _context.Entry(council).Property(e => e.MeetCity).IsModified = true;

            council.MeetState = tblValCouncil.MeetState;
            _context.Entry(council).Property(e => e.MeetState).IsModified = true;

            council.MeetPostalCode = tblValCouncil.MeetPostalCode;
            _context.Entry(council).Property(e => e.MeetPostalCode).IsModified = true;
            //Mailing Address
            council.MailAddress = tblValCouncil.MailAddress;
            _context.Entry(council).Property(e => e.MailAddress).IsModified = true;

            council.MailCity = tblValCouncil.MailCity;
            _context.Entry(council).Property(e => e.MailCity).IsModified = true;

            council.MailState = tblValCouncil.MailState;
            _context.Entry(council).Property(e => e.MailState).IsModified = true;

            council.MailPostalCode = tblValCouncil.MailPostalCode;
            _context.Entry(council).Property(e => e.MailPostalCode).IsModified = true;
            //Meeting times days
            council.BMeetDOW = tblValCouncil.BMeetDOW;
            _context.Entry(council).Property(e => e.BMeetDOW).IsModified = true;

            council.BMeetTime = tblValCouncil.BMeetTime;
            _context.Entry(council).Property(e => e.BMeetTime).IsModified = true;

            council.OMeetDOW = tblValCouncil.OMeetDOW;
            _context.Entry(council).Property(e => e.OMeetDOW).IsModified = true;

            council.OMeetTime = tblValCouncil.OMeetTime;
            _context.Entry(council).Property(e => e.OMeetTime).IsModified = true;

            council.SMeetDOW = tblValCouncil.SMeetDOW;
            _context.Entry(council).Property(e => e.SMeetDOW).IsModified = true;

            council.SMeetTime = tblValCouncil.SMeetTime;
            _context.Entry(council).Property(e => e.SMeetTime).IsModified = true;

            council.Updated = tblValCouncil.Updated;
            _context.Entry(council).Property(e => e.Updated).IsModified = true;

            council.UpdatedBy = tblValCouncil.UpdatedBy;
            _context.Entry(council).Property(e => e.UpdatedBy).IsModified = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblValCouncilExists(id))
                {
                    return BadRequest("Concurrency Issue");
                }
                else
                {
                    return BadRequest("Unknown");
                }
            }

            return Ok(tblValCouncil);
        }
        // PUT: api/TblValCouncils/5
        [HttpPut("Council/{id}")]
        public async Task<ActionResult<TblValCouncil>> UpdateTblValCouncil(int id, [FromBody] TblValCouncil tblValCouncil)
        {
            if (id != tblValCouncil.CNumber)
            {
                // not sure if this would ever happen but we need to communicate back
                // to the calling process what happened
                return BadRequest("Council numbers mismatch");
            }


            try
            {
                _context.Entry(tblValCouncil).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblValCouncilExists(id))
                {
                    return BadRequest("Concurrency Issue");
                }
                else
                {
                    return BadRequest("Unknown");
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Helper.FormatLogEntry(this, ex));
                return BadRequest(ex.Message);
            }

            return Ok(tblValCouncil);
        }

        // DELETE: api/TblValCouncils/5
        [HttpDelete("Council/{id}")]
        public async Task<IActionResult> DeleteTblValCouncil(int id)
        {
            var tblValCouncil = await _context.TblValCouncils.FindAsync(id);
            if (tblValCouncil == null)
            {
                return NotFound();
            }
            try
            {
                _context.TblValCouncils.Remove(tblValCouncil);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { error = "Database error", details = ex.InnerException.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "General error", details = ex.InnerException.Message });
            }

        }

        private bool TblValCouncilExists(int id)
        {
            return _context.TblValCouncils.Any(e => e.CNumber == id);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Models;
using KofCWSC.API.Data;
using Serilog;

namespace KofCWSC.API.Controllers
{
    [Route("")]
    [ApiController]
    public class MemberOfficesController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public MemberOfficesController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/MemberOffices/{id}
        [HttpGet("MemberOffice/{id}")]
        public async Task<ActionResult<IEnumerable<TblCorrMemberOfficeVM>>> GetMemberOffices(int id)
        {
            return _context.Database
                    .SqlQuery<TblCorrMemberOfficeVM>($"EXECUTE uspSYS_GetOfficesForMemberID {id}")
                    .ToList();
            
        }

        // GET: api/MemberOffices/Details/5
        [HttpGet("MemberOffice/Details/{id}")]
        public async Task<ActionResult<TblCorrMemberOffice>> GetMemberOfficeDetails(int id)
        {
            try
            {
                var memberOffice = await _context.TblCorrMemberOffices.FindAsync(id);
                if (memberOffice == null)
                {
                    return NotFound();
                }
                return memberOffice;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "-" + ex.StackTrace);
                return NotFound();  
            }
        }

        // POST: api/MemberOffices
        [HttpPost("MemberOffice")]
        public async Task<ActionResult<TblCorrMemberOffice>> CreateMemberOffice([FromBody] TblCorrMemberOffice memberOffice)
        {
            _context.TblCorrMemberOffices.Add(memberOffice);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMemberOfficeDetails), new { id = memberOffice.Id }, memberOffice);
        }

        // PUT: api/MemberOffices/5
        [HttpPut("MemberOffice/{id}")]
        public async Task<IActionResult> UpdateMemberOffice(int id, [FromBody] TblCorrMemberOffice memberOffice)
        {
            if (id != memberOffice.Id)
            {
                return BadRequest();
            }

            _context.Entry(memberOffice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TblCorrMemberOffices.Any(e => e.Id == id))
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

        // DELETE: api/MemberOffices/5
        [HttpDelete("MemberOffice/{id}")]
        public async Task<IActionResult> DeleteMemberOffice(int id)
        {
            var memberOffice = await _context.TblCorrMemberOffices.FindAsync(id);
            if (memberOffice == null)
            {
                return NotFound();
            }

            _context.TblCorrMemberOffices.Remove(memberOffice);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
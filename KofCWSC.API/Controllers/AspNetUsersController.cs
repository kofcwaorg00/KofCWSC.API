using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using Serilog;

namespace KofCWSC.API.Controllers
{
    [Route("")]
    [ApiController]
    public class AspNetUsersController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public AspNetUsersController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }
        [HttpGet("AspNetUsers")]
        public async Task<ActionResult<IEnumerable<AspNetUser>>> GetAspNetUsers()
        {
            var results = await _context.AspNetUsers.ToListAsync();
            return results;
        }

        [HttpGet("AspNetUsers/{id}")]
        public async Task<ActionResult<TblValCouncil>> GetAspNetUser(string id)
        {
            var results = await _context.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (results == null)
            {
                return NotFound();
            }
            return Ok(results);
        }

        // PUT: api/TblValCouncils/5
        [HttpPut("AspNetUsers/{id}")]
        public async Task<ActionResult<AspNetUser>> UpdateAspNetUser(string id, [FromBody] AspNetUser aspNetUser)
        {
            if (id != aspNetUser.Id)
            {
                // not sure if this would ever happen but we need to communicate back
                // to the calling process what happened
                return BadRequest("Member ID mismatch");
            }

            try
            {
                _context.Entry(aspNetUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Concurrency Issue");
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Helper.FormatLogEntry(this, ex));
                return BadRequest(ex.Message);
            }

            return Ok(aspNetUser);
        }
        // DELETE: api/TblValCouncils/5
        [HttpDelete("AspNetUsers/{id}")]
        public async Task<IActionResult> DeleteAspNetUser(string id)
        {
            var aspNetUser = await _context.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            _context.AspNetUsers.Remove(aspNetUser);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

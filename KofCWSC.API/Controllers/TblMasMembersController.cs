using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Security.Principal;

namespace KofCWSC.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TblMasMembersController : ControllerBase
    {
        //********************************************************************************************
        // June 3, 2024 Tim Philomeno
        // This control works for all CRUD operations
        // ToDo:
        //       Add Authorization and Authentication
        //       Add input validation to prevent SQL Injection Attacks
        //       Will need to figure out how to encrypt/decrypt data to/from the SQL Server
        //       Need to figure out error processing, return OK, NotFoudn, can we transfer
        //           database return messages back to the calling client?
        //********************************************************************************************
        private readonly KofCWSCAPIDBContext _context;

        public TblMasMembersController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/TblMasMembers - we would never use this because it will give us all 17k members, too much data
        [HttpGet("/GetMembers/ByLastName/{lastname}")]
        public async Task<ActionResult<IEnumerable<TblMasMember>>> GetTblMasMembers(string lastname)
        {
            //********************************************************************************************
            // June 3, 2024 Tim Philomeno
            // Didn't want to bring back 17k rows of data so we are using this method "ByLastName" and 
            // sending aaa which will not bring back any data but will allow the UI to be presented
            // and then the user can search by all or part of last name
            //********************************************************************************************
            if (lastname.IsNullOrEmpty())
            {
                lastname = "aaa";
            }
            // the line below would need to be the E model and would go into a variable.  Then foreach 
            // on that variable would be needed to decrypt and fill the non E model and be returned 

            return await _context.TblMasMembers
                .Where(t => t.LastName.Contains(lastname))
                .ToListAsync();
        }

        // GET: api/TblMasMembers/5
        [HttpGet("/GetMember/{id}")]
        public async Task<ActionResult<TblMasMember>> GetTblMasMember(int id)
        {
            //********************************************************************************************
            // June 3, 2024 Tim Philomeno
            // I have chosen to use 3 letter prefixes Get, Upd, Del, Add for my routing
            //********************************************************************************************
            var tblMasMember = await _context.TblMasMembers.FindAsync(id);

            if (tblMasMember == null)
            {
                return NotFound();
            }

            return tblMasMember;
        }

        // PUT: api/TblMasMembers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("/UpdMember/{id}")]
        public async Task<IActionResult> PutTblMasMember(int id, [FromBody] TblMasMember tblMasMember)
        {
            //********************************************************************************************
            // June 3, 2024 Tim Philomeno
            // So it appears that the mvc/ef frameworks automatically takes care of dealing with where
            // the data is.  In this case I added [FromBody] because I am sending the tblMasMember
            // object in the body and we just "know what to do"
            //********************************************************************************************
            if (id != tblMasMember.MemberId)
            {
                return BadRequest();
            }

            _context.Entry(tblMasMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblMasMemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return Content("Untrapped Error");
                }
            }

            return Ok();
        }

        // POST: api/TblMasMembers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/NewMember")]
        public async Task<ActionResult<TblMasMember>> PostTblMasMember([FromBody]TblMasMember tblMasMember)
        {
            //********************************************************************************************
            // June 3, 2024 Tim Philomeno
            // Here is where I ran into issues with web.config and WEBDAV and AspNetCor modules requiring
            // POST, PUT, and DELETE
            //********************************************************************************************
            _context.TblMasMembers.Add(tblMasMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblMasMember", new { id = tblMasMember.MemberId }, tblMasMember);
        }

        // DELETE: api/TblMasMembers/5
        [HttpDelete("/DelMember/{id}")]
        public async Task<IActionResult> DeleteTblMasMember(int id)
        {
            //********************************************************************************************
            // June 3, 2024 Tim Philomeno
            //********************************************************************************************
            var tblMasMember = await _context.TblMasMembers.FindAsync(id);
            if (tblMasMember == null)
            {
                return NotFound();
            }

            _context.TblMasMembers.Remove(tblMasMember);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblMasMemberExists(int id)
        {
            //********************************************************************************************
            // June 3, 2024 Tim Philomeno
            // Looks like a "helper" function used in this class
            //********************************************************************************************
            return _context.TblMasMembers.Any(e => e.MemberId == id);
        }
    }
}

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
using Serilog;

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
            Log.Information("DBContext Created");
        }

        // GET: api/TblMasMembers - we would never use this because it will give us all 17k members, too much data
        [HttpGet("/GetMembers/ByLastName/{lastname}")]
        public async Task<ActionResult<IEnumerable<TblMasMember>>> GetTblMasMembers(string lastname)
        {
            Log.Information("Starting GetMembers/ByLastName/" + lastname);
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
            try
            {
                return await _context.TblMasMembers
                .Where(t => t.LastName.Contains(lastname))
                .ToListAsync();
            }
            catch (Exception ex)
            {

                Log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
            
        }

        // GET: api/TblMasMembers/5
        [HttpGet("/GetMember/{id}")]
        public async Task<ActionResult<TblMasMember>> GetTblMasMember(int id)
        {
            Log.Information("Starting Getting Member " + id);
            //********************************************************************************************
            // June 3, 2024 Tim Philomeno
            // I have chosen to use 3 letter prefixes Get, Upd, Del, Add for my routing
            //********************************************************************************************
            var tblMasMember = await _context.TblMasMembers.FindAsync(id);

            if (tblMasMember == null)
            {
                Log.Fatal("GetMember ID " + id + " Not found");
                return NotFound();
            }

            return tblMasMember;
        }

        // PUT: api/TblMasMembers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("/UpdMember/{id}")]
        public async Task<IActionResult> PutTblMasMember(int id, [FromBody] TblMasMember tblMasMember)
        {
            Log.Information("Starting UpdMember " + id);
            //********************************************************************************************
            // June 3, 2024 Tim Philomeno
            // So it appears that the mvc/ef frameworks automatically takes care of dealing with where
            // the data is.  In this case I added [FromBody] because I am sending the tblMasMember
            // object in the body and we just "know what to do"
            //********************************************************************************************
            if (id != tblMasMember.MemberId)
            {
                Log.Fatal("Member ID " + id + " Not found");
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
                    Log.Fatal("Concurrencty Issue with Member ID " + id + " Not found");
                    return NotFound();
                }
                else
                {
                    Log.Fatal("Concurrenty Issue");
                    return Content("Untrapped Error");
                }
            }
            catch (Exception ex)
            {
                Log.Fatal("From Catch in UpdMember" + ex.Message + " " + ex.InnerException);
            }

            return Ok();
        }

        // POST: api/TblMasMembers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/NewMember")]
        public async Task<ActionResult<TblMasMember>> PostTblMasMember([FromBody]TblMasMember tblMasMember)
        {
            Log.Information("Starting NewMember");
            //********************************************************************************************
            // June 3, 2024 Tim Philomeno
            // Here is where I ran into issues with web.config and WEBDAV and AspNetCor modules requiring
            // POST, PUT, and DELETE
            //********************************************************************************************
            try
            {
                _context.TblMasMembers.Add(tblMasMember);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTblMasMember", new { id = tblMasMember.MemberId }, tblMasMember);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message + " " + ex.InnerException);
                return NoContent();
            }
        }

        // DELETE: api/TblMasMembers/5
        [HttpDelete("/DelMember/{id}")]
        public async Task<IActionResult> DeleteTblMasMember(int id)
        {
            Log.Information("Starting DelMember ID " + id);
            //********************************************************************************************
            // June 3, 2024 Tim Philomeno
            //********************************************************************************************
            var tblMasMember = await _context.TblMasMembers.FindAsync(id);
            if (tblMasMember == null)
            {
                Log.Fatal("DelMember ID " + id + " Not found");
                return NotFound();
            }

            try
            {
                Log.Information("before remove");
                _context.TblMasMembers.Remove(tblMasMember);
                Log.Information("Before SaveChanges");
                await _context.SaveChangesAsync();
                Log.Information("Success Deleting " + id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
                return NoContent();
            }
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

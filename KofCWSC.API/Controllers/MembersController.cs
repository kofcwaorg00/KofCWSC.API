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
using System.Text.RegularExpressions;

namespace KofCWSC.API.Controllers
{
    //[Route("[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
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
        // 6/24/2024 Tim Philmoeno
        // I have removed the Route("[controller]") decoration to keep the routes more aligned with
        // best practices of just using the Entity in combination of the HTTP type is all we need
        // HttpGet LastName a collection of Member(s) by Last Name
        // HttpGet (id) get a single member
        // HttpPut (id) update a single member
        // HttpDelete (id) delete a single member
        // HttpPost add a member
        //********************************************************************************************
        private readonly KofCWSCAPIDBContext _context;

        public MembersController(KofCWSCAPIDBContext context)
        {
            _context = context;
            Log.Information("DBContext Created");
        }

        // GET: api/TblMasMembers - we would never use this because it will give us all 17k members, too much data
        [HttpGet("Members/LastName/{lastname}")]
        public async Task<ActionResult<IEnumerable<TblMasMember>>> GetTblMasMembers(string lastname)
        {
            Log.Information("Starting GetMembers/ByLastName/" + lastname);
            if (!IsLastNameValid(lastname))
            {
                Log.Error("Invalid Last Name");
                return BadRequest("Invalid Characters in Last Name");
            }
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

            try
            {
                return await _context.TblMasMembers
                .Where(t => t.LastName.Contains(lastname))
                .ToListAsync();
            }
            catch (Exception ex)
            {

                Log.Error("From " + GetType() + System.Reflection.MethodBase.GetCurrentMethod() + " " + ex.Message + " " + ex.InnerException);
                throw new Exception("From " + GetType() + " " +ex.Message);
            }
            
        }

        // GET: api/TblMasMembers/5
        //[HttpGet("/GetMember/{id}")]
        [HttpGet("Member/{id}")]
        public async Task<ActionResult<TblMasMember>> GetTblMasMember(int id)
        {
            
            Log.Information("Starting Getting Member " + id);
            if (!IsMemberIDValid(id))
            {
                Log.Fatal("GetMember ID " + id + " Not found");
                return NotFound();
            }

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
        [HttpPut("/Member/{id}")]
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
        [HttpPost("/Member")]
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
                Log.Error("From " + GetType() + System.Reflection.MethodBase.GetCurrentMethod() + " " + ex.Message + " " + ex.InnerException);
                return NoContent();
            }
        }

        // DELETE: api/TblMasMembers/5
        [HttpDelete("/Member/{id}")]
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
                Log.Fatal(ex.Message + " " + ex.InnerException);
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
        private bool IsMemberIDValid(int id)
        {
            //********************************************************************************************
            // 6/25/2024 Tim Philomeno
            // Rules for incoming parameter id
            // must be an integer (this should be taken care of becasue of our parameter data type)
            // must be between 1 and 100,000, 100,000 is arbitrary based on the current identity column
            // max value of 21,182
            //********************************************************************************************
            if (id == 0)
            {
                return false;
            }
            if (id < 0)
            {
                return false;
            }
            if (id > 100000)
            {
                return false;
            }
            return true;
        }
        private bool IsLastNameValid(string LastName)
        {
            //********************************************************************************************
            // 6/25/2024 Tim Philomeno
            // Rules for incoming parameter LastName
            // must be a string (this should be taken care of becasue of our parameter data type)
            // must be between 1 and 50 which is the lenght of the tbl_MasMembers.LastName
            // must be a-z period space comma
            //********************************************************************************************
            if (LastName.Length == 0)
            {
                return false;
            }
            if (LastName.Length > 50)
            {
                return false;
            }

            string rxPat = (@"^[a-zA-Z.,\s]+$");
            
            if(!Regex.IsMatch(LastName, rxPat))
            {
                return false;
            }

            return true;
        }
    }
}

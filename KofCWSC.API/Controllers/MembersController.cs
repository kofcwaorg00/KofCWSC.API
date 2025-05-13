using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Data;
using KofCWSC.API.Models;
using KofCWSC.API.Utils;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text.RegularExpressions;
using System.ComponentModel;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Reflection;


namespace KofCWSC.API.Controllers
{
    [Route("")]
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
            Log.Information(Helper.FormatLogEntry(this, new Exception("DBContext Created")));
        }

        [HttpGet("Member/KofCID/{KofCID}")]
        public async Task<ActionResult<TblMasMember>> GetMemberByKofCID(int KofCID)
        {
            try
            {
                return _context.TblMasMembers
                .Where(t => t.KofCid == KofCID).FirstOrDefault();
            }
            catch (Exception ex)
            {

                Log.Error("Invalid Last Name");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Members/KofCID/{KofCID}")]
        public async Task<ActionResult<IEnumerable<TblMasMember>>> GetMembersByKofCID(int KofCID)
        {
            try
            {
                return await _context.TblMasMembers
                .Where(t => t.KofCid == KofCID)
                .ToListAsync();
            }
            catch (Exception ex)
            {

                Log.Error("Invalid Last Name");
                return BadRequest(ex.Message);
            }
        }

        // GET: api/TblMasMembers - we would never use this because it will give us all 17k members, too much data
        [HttpGet("Members/LastName/{lastname}")]
        public async Task<ActionResult<IEnumerable<TblMasMember>>> GetMembers(string lastname)
        {
            Log.Information("Starting GetMembers/ByLastName/" + lastname);
            if (!IsLastNameValid(lastname))
            {
                Log.Error(Helper.FormatLogEntry(this, new Exception("Invalid Last Name")));
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

                Log.Error("Invalid Last Name");
                return BadRequest(ex.Message);
            }

        }

        // GET: api/TblMasMembers/5
        //[HttpGet("/GetMember/{id}")]
        [HttpGet("Member/{id}")]
        public async Task<ActionResult<TblMasMember>> GetMembers(int id)
        {

            if (!IsMemberIDValid(id))
            {
                Log.Fatal("GetMember ID " + id + " Not found");
                return NotFound();
            }

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
        public async Task<IActionResult> Member(int id, [FromBody] TblMasMember tblMasMember)
        {
            //********************************************************************************************
            // June 3, 2024 Tim Philomeno
            // So it appears that the mvc/ef frameworks automatically takes care of dealing with where
            // the data is.  In this case I added [FromBody] because I am sending the tblMasMember
            // object in the body and we just "know what to do"
            //********************************************************************************************
            try
            {
                if (id != tblMasMember.MemberId)
                {
                    Log.Fatal("Member ID " + id + " Not found");
                    return BadRequest($"Member ID {id} Not Found");
                }
                FormatMemberDataToSpec(ref tblMasMember);
                //********************************************************************************************
                // 5/12/2025 Tim Philomeno
                // The follwing block of code will dynmaically set the LastUpdated/By data for any process
                // that flow through.  This is specific to the tbl_MasMembers table only.  The incoming 
                // LastUpdated and LastUpdatedBy will contain the data from the calling process.
                //----------------------------------------------------------------------------------------------
                var existingMember = _context.TblMasMembers.Find(tblMasMember.MemberId);
                if (existingMember == null)
                {
                    return NotFound();
                }
                var properties = typeof(TblMasMember).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var allowedProperties = new List<string> { "Prefix", "FirstName", "NickName", "MI", "LastName", "Suffix", "AddInfo1", "Address", "City", "State", "PostalCode", "Phone", "WifesName", "AddInfo2", "FaxNumber", "Council", "Assembly", "Circle", "Email", "Deceased", "CellPhone", "Bulletin" };

                // spin through all properties
                foreach (var prop in properties)
                {
                    // only run the updated and updatedby on those that are not tracked
                    if (!prop.Name.ToLower().Contains("updated"))
                    {
                        var newValue = prop.GetValue(tblMasMember);
                        var existingValue = prop.GetValue(existingMember);
                        // compare the 2 values
                        if (!object.Equals(newValue, existingValue))
                        {
                            // Update the main value
                            prop.SetValue(existingMember, newValue);
                            _context.Entry(existingMember).Property(prop.Name).IsModified = true;
                            // only update the allowed properties
                            if (allowedProperties.Contains(prop.Name))
                            {
                                string updproperty = prop.Name;
                                string updUpdated = $"{updproperty}Updated";
                                string updUpdatedBy = $"{updproperty}UpdatedBy";
                                typeof(TblMasMember).GetProperty(updUpdated)?.SetValue(existingMember, tblMasMember.LastUpdated);
                                typeof(TblMasMember).GetProperty(updUpdatedBy)?.SetValue(existingMember, tblMasMember.LastUpdatedBy.ToString());
                                _context.Entry(existingMember).Property(updUpdated).IsModified = true;
                                _context.Entry(existingMember).Property(updUpdatedBy).IsModified = true;
                            }
                        }
                    }
                }
                //********************************************************************************************
                //_context.Entry(existingMember).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblMasMemberExists(id))
                {
                    return BadRequest($"Concurrency Issue Updating Member ID {id}");
                }
                else
                {
                    Log.Fatal("Concurrenty Issue");
                    return BadRequest("Untrapped Concurrency Issue");
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(Helper.FormatLogEntry(this, ex));
                return BadRequest("Untrapped Error in Member PUT");
            }

            return Ok();
        }

        // POST: api/TblMasMembers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/Member")]
        public async Task<ActionResult<TblMasMember>> Member([FromBody] TblMasMember tblMasMember)
        {
            //********************************************************************************************
            // June 3, 2024 Tim Philomeno
            // Here is where I ran into issues with web.config and WEBDAV and AspNetCor modules requiring
            // POST, PUT, and DELETE
            //********************************************************************************************
            try
            {
                // Call the UserService to create a new user
                FormatMemberDataToSpec(ref tblMasMember);
                _context.TblMasMembers.Add(tblMasMember);
                await _context.SaveChangesAsync();
                return Ok();
                //return CreatedAtAction("GetTblMasMember", new { id = tblMasMember.MemberId }, tblMasMember);
            }
            catch (DbUpdateException sqlex)
            {
                Log.Error(Helper.FormatLogEntry(this, sqlex));
                return BadRequest(sqlex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(Helper.FormatLogEntry(this, ex));
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/TblMasMembers/5
        [HttpDelete("/Member/{id}")]
        public async Task<IActionResult> DeleteMember(int id)
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
                _context.TblMasMembers.Remove(tblMasMember);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Information("Success Deleting " + id + ex.Message);
                return NoContent();
            }
        }
        //[HttpGet("hidden-action")]
        //[ApiExplorerSettings(IgnoreApi = true)]  // This action will be ignored by Swagger

        [HttpGet("IsKofCMember/{id}")]
        public async Task<ActionResult<TblMasMember>> IsKofCMember(int id)
        {
            var results = _context.TblMasMembers.Where(p => p.KofCid == id).FirstOrDefault();
            if (results == null)
            {
                //return NotFound(); // I added this so the API Helper can handle a "" json string
                return results; // I added this so the API Helper can handle a "" json string
            }
            else
            {
                return results;
            }

        }
        // GET: api/TblMasMembers - we would never use this because it will give us all 17k members, too much data
        //****************************************************************************************************************
        // 2/15/2025 Tim Philomeno
        // This takes over an hour and 1/2 to run on our data.  Should probably not need to run it after one time
        // because the CREATE/UPDATE methods will format on the fly and we now have the button on the Manage Members
        // index to do them individually.  I ran it today
        //---------------------------------------------------------------------------------------------------------------
        [HttpGet("Members/FormatData")]
        public async Task<ActionResult> FormatMemberData()
        {
            List<TblMasMember> myMem = await _context.TblMasMembers.ToListAsync();
            // now spin through and format the data
            foreach (var mem in myMem)
            {
                var myMember = mem;
                FormatMemberDataToSpec(ref myMember);
                _context.Entry(myMember).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return Ok();
        }
        // GET: api/TblMasMembers - we would never use this because it will give us all 17k members, too much data
        [HttpGet("Members/FormatData/{id}")]
        public async Task<ActionResult> FormatMemberData(int id)
        {
            var myMem = await _context.TblMasMembers
                .Where(e => e.MemberId == id)
                .FirstOrDefaultAsync();
            // now update the spec and save
            if (myMem == null)
            {
                return NotFound();
            }
            else
            {
                FormatMemberDataToSpec(ref myMem);
                _context.Entry(myMem).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(myMem.LastName);
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

            string rxPat = (@"^[0-9a-zA-Z.,\s]+$");

            if (!Regex.IsMatch(LastName, rxPat))
            {
                return false;
            }

            return true;
        }
        private static void FormatMemberDataToSpec(ref TblMasMember tblMasMember)
        {
            if (!tblMasMember.FirstName.IsNullOrEmpty()) { tblMasMember.FirstName = tblMasMember.FirstName.Trim(); }
            if (!tblMasMember.LastName.IsNullOrEmpty()) { tblMasMember.LastName = tblMasMember.LastName.Trim(); }
            if (!tblMasMember.Mi.IsNullOrEmpty()) { tblMasMember.Mi = tblMasMember.Mi.Trim(); }
            if (!tblMasMember.Phone.IsNullOrEmpty()) { tblMasMember.Phone = Helper.FormatPhoneNumber(tblMasMember.Phone); }
            if (!tblMasMember.Address.IsNullOrEmpty()) { tblMasMember.Address = tblMasMember.Address.ToUpper(); }
            if (!tblMasMember.City.IsNullOrEmpty()) { tblMasMember.City = tblMasMember.City.ToUpper(); }
            if (!tblMasMember.State.IsNullOrEmpty()) { tblMasMember.State = tblMasMember.State.ToUpper(); }
            if (!tblMasMember.Email.IsNullOrEmpty()) { tblMasMember.Email = tblMasMember.Email.ToUpper(); }
        }
    }
}

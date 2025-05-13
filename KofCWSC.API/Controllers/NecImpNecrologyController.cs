using KofCWSC.API.Data;
using KofCWSC.API.Models;
using KofCWSC.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KofCWSC.API.Controllers
{
    public class NecImpNecrologyController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;
        public NecImpNecrologyController(KofCWSCAPIDBContext context)
        {
            _context = context;
            Log.Information(Helper.FormatLogEntry(this, new Exception("DBContext Created")));
        }
        // GET: NecImpNecrology
        [HttpGet("Necrologies")]
        public async Task<ActionResult<IEnumerable<NecImpNecrology>>> GetNec()
        {
            try
            {
                var results = await _context.TblNecImpNecrologies.ToListAsync();
                if (!results.Any())
                {
                    return NoContent();
                }
                return Ok(results); // Use Ok() for successful responses.
            }
            catch (Exception ex)
            {
                Utils.Helper.FormatLogEntry(this, ex);
                return BadRequest(ex.Message); // Include exception details if appropriate.
            }
        }
        // GET: NecImpNecrology
        [HttpGet("Necrology")]
        public async Task<ActionResult<IEnumerable<NecImpNecrologyVM>>> GetNecrology()
        {
            try
            {
                var results = await _context.Database.SqlQuery<NecImpNecrologyVM>($"EXECUTE uspNEC_GetNecrology").ToListAsync();
                if (!results.Any())
                {
                    // 5/12/2025 Tim Philomeno
                    // NoContent() will return a NULL model that can be handled in the corresponding VIEW, i.e. No Records Found
                    return NoContent();
                }
                return Ok(results); // Use Ok() for successful responses.
            }
            catch (Exception ex)
            {
                Utils.Helper.FormatLogEntry(this, ex);
                return Problem(detail: ex.Message, statusCode: 400,title:"Unexpected Error in API",type:"GET/Necrology",instance:"KofCWSC API");
                //return BadRequest(ex.Message); // Include exception details if appropriate.
            }
        }

        [HttpPost("ImpNecrology")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<NecImpNecrology>> ImpNecrology([FromBody] List<NecImpNecrology>  impNecrologies)
        {
            string myRetMess = "";
            try
            {

                if (ModelState.IsValid)
                {
                    // first delete the existing table data
                    //_context.Database.ExecuteSqlRaw("TRUNCATE TABLE [tblCVN_ImpDelegates]");
                    // second import the incoming data
                    foreach (var myDel in impNecrologies)
                    {
                        try
                        {
                            // next line should clear context so the previous adds are gone
                            _context.ChangeTracker.Clear();

                            _context.TblNecImpNecrologies.Add(myDel);
                            await _context.SaveChangesAsync();
                            myRetMess += $"Adding New Record {myDel.DecFname} {myDel.DecLname};";
                        }
                        catch (DbUpdateException ex)
                        {
                            if (ex.InnerException.Message.Contains("duplicate key"))
                            {
                                myRetMess += ""; //$"Skipping Duplicate submissions";
                                // do nothing just ignore
                            }
                            else
                            {
                                myRetMess += ""; // $"Skipping others ";
                                // do something
                            }
                        }
                        catch (Exception ex) { 
                            Utils.Helper.FormatLogEntry(this, ex);
                            return BadRequest(ex.Message);
                        }
                    }
                }
                else
                {
                   
                    return BadRequest(myRetMess);
                }
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex.Message + " - " + ex.InnerException);
                return BadRequest($"Error saving changes - {ex.Message}");
            }
            if (myRetMess.Length == 0)
            {
                myRetMess = "No new Necrology Records were Imported";
            }
            //return Ok("Import Successful");
            return Json(myRetMess);
        }
        [HttpGet("UpdateDeceased/{KofCID}")]
        public int UpdateDeceased(int KofCID)
        {
            var rowsAffected =  _context.Database.ExecuteSql($"EXECUTE [uspNEC_UpdateDeceased] {KofCID}");
            return rowsAffected;
        }
    }
}

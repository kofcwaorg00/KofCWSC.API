using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Utils;
using Serilog;

namespace KofCWSC.API.Controllers
{
    public class CvnLocationsController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public CvnLocationsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }
       
        // GET: api/TblValCouncils
        [HttpGet("Locations")]
        public async Task<ActionResult<IEnumerable<CvnLocation>>> GetLocations()
        {
            return await _context.TblCvnMasLocations
                .OrderBy(x => x.Location)
                .ToListAsync();
        }

        // POST: api/TblValCouncils
        [HttpPost("Locations")]
        public async Task<ActionResult<CvnLocation>> CreateCvnLocation([FromBody] CvnLocation cvnLocation)
        {
            _context.TblCvnMasLocations.Add(cvnLocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateCvnLocation), new { id = cvnLocation.Id }, cvnLocation);
        }

        // GET: api/cvnLocation/5
        [HttpGet("Locations/{id}")]
        public async Task<ActionResult<CvnLocation>> GetCvnLocation(int id)
        {
            var cvnLocation = await _context.TblCvnMasLocations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cvnLocation == null)
            {
                return NotFound();
            }
            return Ok(cvnLocation);
        }

        // DELETE: api/tblCvnLocation/5
        [HttpDelete("Locations/{id}")]
        public async Task<IActionResult> DeleteCvnLocation(int id)
        {
            try
            {
                var tblCvnLocation = await _context.TblCvnMasLocations.FindAsync(id);
                if (tblCvnLocation == null)
                {
                    return NotFound();
                }

                _context.TblCvnMasLocations.Remove(tblCvnLocation);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Helper.FormatLogEntry(this, ex));
                //trying to find a way to communicate messages back to the calling
                // controller.  This doesn't work either
                return new ObjectResult(new { message = "Cannot Delete an Item that is in use!" })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
                
                //return BadRequest("Cannot Delete an Item that is in use!");
            }
            
        }


    }
}

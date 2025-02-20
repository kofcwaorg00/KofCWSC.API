using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KofCWSC.API.Controllers
{
    public class CvnMileageController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public CvnMileageController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/TblValCouncils
        [HttpGet("MileageForCouncils")]
        public async Task<ActionResult<IEnumerable<CvnMileageC>>> GetMileageForCouncils()
        {
            try
            {
                var results = await _context.Database.SqlQuery<CvnMileageC>($"EXECUTE uspCVN_GetMileageCouncils").ToListAsync();

                return results;
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Helper.FormatLogEntry(this, ex));
                return Empty;
            }
        }

        // GET: api/TblValCouncils
        [HttpGet("Mileage")]
        public async Task<ActionResult<IEnumerable<CvnMileage>>> GetMileage()
        {
            return await _context.TblCvnMasMileages
                .OrderBy(x => x.Location)
                .ToListAsync();
        }

        // GET: api/TblValCouncils/5
        [HttpGet("Mileage/{id}")]
        public async Task<ActionResult<CvnMileage>> GetMileage(int id)
        {
            var cvnMileage = await _context.TblCvnMasMileages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cvnMileage == null)
            {
                return NotFound();
            }
            return Ok(cvnMileage);
        }
        // GET: api/TblValCouncils/5
        [HttpGet("MileageForCouncil/{council}/{location}")]
        public async Task<ActionResult<CvnMileage>> GetMileageForCouncil(int council,string location)
        {
            var cvnMileage = await _context.TblCvnMasMileages
                .Where(e => e.Council == council && e.Location == location)
                .FirstOrDefaultAsync();
            if (cvnMileage == null)
            {
                return cvnMileage;
            }
            return Ok(cvnMileage);
        }
        // POST: api/TblValCouncils
        [HttpPost("Mileage")]
        public async Task<ActionResult<TblValCouncil>> CreateCvnMileage([FromBody]CvnMileage cvnMileage)
        {
            _context.TblCvnMasMileages.Add(cvnMileage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateCvnMileage), new { id = cvnMileage.Id }, cvnMileage);
        }
        // PUT: api/TblValCouncils/5
        [HttpPut("Mileage/{id}")]
        public async Task<ActionResult<CvnMileage>> UpdateCvnMileage(int id,[FromBody] CvnMileage cvnMileage)
        {
            if (id != cvnMileage.Id)
            {
                // not sure if this would ever happen but we need to communicate back
                // to the calling process what happened
                return BadRequest("Council numbers mismatch");
            }
            _context.Entry(cvnMileage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CvnMileageExists(id))
                {
                    return BadRequest("Concurrency Issue");
                }
                else
                {
                    return BadRequest("Unknown");
                }
            }

            return Ok(cvnMileage);
        }
        // DELETE: api/CvnMileage/5
        [HttpDelete("Mileage/{id}")]
        public async Task<IActionResult> DeleteCvnMileage(int id)
        {
            var cvnMileage = await _context.TblCvnMasMileages.FindAsync(id);
            if (cvnMileage == null)
            {
                return NotFound();
            }

            _context.TblCvnMasMileages.Remove(cvnMileage);
            await _context.SaveChangesAsync();

            return Ok();
        }
        private bool CvnMileageExists(int id)
        {
            return _context.TblCvnMasMileages.Any(e => e.Id == id);
        }
    }
}

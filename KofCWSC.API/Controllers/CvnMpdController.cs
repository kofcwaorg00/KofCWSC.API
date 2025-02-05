using KofCWSC.API.Models;
using KofCWSC.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KofCWSC.API.Controllers
{
    public class CvnMpdController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public CvnMpdController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }
        // GET: 
        [HttpGet("PrimeCouncilDelegatesAndDDDays")]
        public async Task<ActionResult<int>> PrimeCouncilDelegatesAndDDDays()
        {
            int RowsAffected = _context.Database.ExecuteSql($"EXECUTE uspCVN_PrimeCouncilDelegatesAndDDDays");
            return Ok(RowsAffected);
        }
        [HttpGet("GetMPDChecks/{id}")]
        public async Task<ActionResult<IEnumerable<CvnMpd>>> GetMPDChecks(int id)
        {
            try
            {
                var results = await _context.Database
                       .SqlQuery<CvnMpd>($"[uspCVN_GetMPDChecks] {id}").ToListAsync();
                return results;
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Helper.FormatLogEntry(this, ex));
                return BadRequest("Error");
            }

        }
        // GET: api/TblValCouncils
        [HttpGet("MPD/{id}")]
        public async Task<ActionResult<IEnumerable<CvnMpd>>> GetMPD(int id)
        {
            int RowsAffected = _context.Database.ExecuteSql($"EXECUTE [uspCVN_CreateCheckBatch] {id}");
            if (id == 3) // DDs
            {
                return await _context.TblCvnTrxMpds
                    .Where(x => x.GroupID == 3)
                    .OrderBy(x => x.District)
                    .ToListAsync();
            }
            else if(id == 25) // Delegates
            {
                return await _context.TblCvnTrxMpds
                    .Where(x => x.GroupID == 25)
                    .OrderBy(x => x.District)
                    .ToListAsync();
            }
            else // most likley 0
            {
                return await _context.TblCvnTrxMpds
                    .OrderBy(x => x.District)
                    .ToListAsync();
            }
        }
        // GET: api/TblValCouncils
        [HttpGet("MPD/GetCheckBatch/{id}")]
        public async Task<ActionResult<IEnumerable<CvnMpd>>> GetCheckBatch(int id)
        {
            if (id == 0)
            {
                throw new Exception("MPD/GetCheckBatch was sent a 0");
            }
            try
            {
                int RowsAffected = _context.Database.ExecuteSql($"EXECUTE uspCVN_CreateCheckBatch {id}");
                return await _context.Database.SqlQuery<CvnMpd>($"EXECUTE uspCVN_GetCheckBatch {id}").ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Helper.FormatLogEntry(this, ex));
                return BadRequest();
            }
            
        }
    }
}

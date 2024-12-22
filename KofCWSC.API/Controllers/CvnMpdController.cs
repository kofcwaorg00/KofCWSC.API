using KofCWSC.API.Models;
using KofCWSC.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KofCWSC.API.Controllers
{
    public class CvnMpdController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public CvnMpdController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }
        // GET: api/TblValCouncils
        [HttpGet("MPD")]
        public async Task<ActionResult<IEnumerable<CvnMpd>>> GetMPD()
        {
            int RowsAffected = _context.Database.ExecuteSql($"EXECUTE [uspCVN_CreateCheckBatch]");
            return await _context.TblCvnTrxMpds
                .OrderBy(x => x.District)
                .ToListAsync();
        }
    }
}

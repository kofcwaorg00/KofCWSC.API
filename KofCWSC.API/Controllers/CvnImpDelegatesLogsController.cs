using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KofCWSC.API.Controllers
{
    [Route("")]
    [ApiController]
    public class CvnImpDelegatesLogsController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;
        public CvnImpDelegatesLogsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: ImpDelegate
        [HttpGet("ImpDelegateLogs")]
        public async Task<ActionResult<IEnumerable<CvnImpDelegatesLog>>> GetImpDelegateLogs()
        {
            var results = await _context.CvnImpDelegatesLogs
                .ToListAsync();
            if (results.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return results;
            }

        }
    }
}

using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KofCWSC.API.Controllers
{
    public class EmailGroupsController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;
        public EmailGroupsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: api/SP/GetDDs
        [HttpGet("GetDDL/{GroupID}/{NextYear}")]
        public async Task<ActionResult<IEnumerable<EmailGroups>>> GetDDL(int GroupID,int NextYear = 0)
        {
            return await _context.Database.SqlQuery<EmailGroups>($"EXECUTE uspWEB_GetDDL {GroupID}, {NextYear}").ToListAsync();
        }
    }
}

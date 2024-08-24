using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;

namespace KofCWSC.API.Controllers
{
    [Route("")]
    [ApiController]
    public class SPController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public SPController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: GetAssys
        [HttpGet("GetAssys")]
        public async Task<ActionResult<IEnumerable<SPGetAssysView>>> GetAssys()
        {
            return await _context.Database.SqlQuery<SPGetAssysView>($"EXECUTE uspWEB_GetAssys").ToListAsync();
        }
        // GET: GetCouncils
        [HttpGet("GetCouncils")]
        public async Task<ActionResult<IEnumerable<SPGetCouncilsView>>> GetCouncils()
        {
            return await _context.Database.SqlQuery<SPGetCouncilsView>($"EXECUTE uspWEB_GetCouncils").ToListAsync();
        }

        // GET: GetSOS
        [HttpGet("GetSOSView")]
        public async Task<ActionResult<IEnumerable<SPGetSOSView>>> GetSOSView()
        {
            // Run Sproc
            return await _context.Database.SqlQuery<SPGetSOSView>($"EXECUTE uspWEB_GetSOSView").ToListAsync();
        }

        // GET: api/SP/GetBulletins
        [HttpGet("GetBulletins")]
        public async Task<ActionResult<IEnumerable<SPGetBulletins>>> GetBulletins()
        {
            return await _context.Database.SqlQuery<SPGetBulletins>($"EXECUTE uspSYS_GetBulletins").ToListAsync();
        }

        // GET: api/SP/GetEmailAlias
        [HttpGet("GetEmailAlias")]
        public async Task<ActionResult<IEnumerable<SPGetEmailAlias>>> GetEmailAlias()
        {
            return await _context.Database.SqlQuery<SPGetEmailAlias>($"EXECUTE uspWEB_GetEmailAlias").ToListAsync();
        }

        // GET: api/SP/GetChairmen
        [HttpGet("GetChairmen")]
        public async Task<ActionResult<IEnumerable<SPGetChairmen>>> GetChairmen()
        {
            return await _context.Database.SqlQuery<SPGetChairmen>($"EXECUTE uspWEB_GetChairmen 0").ToListAsync();
        }

        // GET: api/SP/GetChairmanInfoBlock/{id}
        [HttpGet("GetChairmanInfoBlock/{id}")]
        public async Task<ActionResult<IEnumerable<SPGetChairmanInfoBlock>>> GetChairmanInfoBlock(int id)
        {
            //Check to see if we have a valid chairman id
            var myChairmenList = await _context.Database
                   .SqlQuery<SPGetChairmenId>($"EXECUTE uspWEB_IsChairman {id} ")
                   .ToListAsync();

            if (myChairmenList.Count() == 0)
            {
                return NotFound();
            }
            return await _context.Database.SqlQuery<SPGetChairmanInfoBlock>($"EXECUTE uspWEB_GetChairmanInfoBlock {id}").ToListAsync();

        }

        // GET: api/SP/GetDDs
        [HttpGet("GetDDs")]
        public async Task<ActionResult<IEnumerable<SPGetDDs>>> GetDDs()
        {
            return await _context.Database.SqlQuery<SPGetDDs>($"EXECUTE uspWEB_GetDDs 0").ToListAsync();
        }

        // GET: api/SP/FourthDegreeOfficers
        [HttpGet("FourthDegreeOfficers")]
        public async Task<ActionResult<IEnumerable<SPGetChairmanInfoBlock>>> GetFourthDegreeOfficers()
        {
            return await _context.Database.SqlQuery<SPGetChairmanInfoBlock>($"EXECUTE uspWEB_GetFourthDegreeOfficers").ToListAsync();
        }
    }
}

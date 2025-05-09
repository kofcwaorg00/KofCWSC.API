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
    public class SPController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public SPController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }
        // GET: NextTempID
        [HttpGet("GetNextTempID")]
        public IQueryable<string> GetNextTempID()
        {
            var myID = _context.Database.SqlQuery<string>($"EXECUTE uspWSC_GetNextTempID");
            return myID;
            
        }
        // GET: FraternalYear
        [HttpGet("GetFratYear/{NextYear}")]
        public async Task<ActionResult> GetFratYear(int NextYear)
        {
            var myYear = _context.funSYS_GetBegFratYearN.FromSqlInterpolated($"SELECT dbo.funSYS_GetBegFratYearN({NextYear}) as FratYear").FirstOrDefault().FratYear;
            return Json(myYear);
        }
        // GET: MemberName
        [HttpGet("GetMemberName/{id}")]
        public async Task<ActionResult> GetMemberName(int id)
        {
            var myName = _context.funSYS_BuildName.FromSqlInterpolated($"SELECT dbo.funSYS_BuildName({id},0,'N') as MemberName").FirstOrDefault().MemberName;
            return Json(myName);
        }

        // GET: GetAssys
        [HttpGet("GetAssys/{NextYear}")]
        public async Task<ActionResult<IEnumerable<SPGetAssysView>>> GetAssys(int NextYear = 0)
        {
            return await _context.Database.SqlQuery<SPGetAssysView>($"EXECUTE uspWEB_GetAssys {NextYear}").ToListAsync();
        }
        // GET: GetCouncils
        [HttpGet("GetCouncils/{NextYear}")]
        public async Task<ActionResult<IEnumerable<SPGetCouncilsView>>> GetCouncils(int NextYear = 0)
        {
            return await _context.Database.SqlQuery<SPGetCouncilsView>($"EXECUTE uspWEB_GetCouncils {NextYear}").ToListAsync();
        }

        // GET: GetSOS
        [HttpGet("GetSOSView/{NextYear}")]
        public async Task<ActionResult<IEnumerable<SPGetSOSView>>> GetSOSView(int NextYear = 0)
        {
            // Run Sproc
            return await _context.Database.SqlQuery<SPGetSOSView>($"EXECUTE uspWEB_GetSOSView {NextYear}").ToListAsync();
        }

        // GET: api/SP/GetBulletins OBSOLETE
        //[HttpGet("GetBulletins")]
        //public async Task<ActionResult<IEnumerable<SPGetBulletins>>> GetBulletins()
        //{
        //    return await _context.Database.SqlQuery<SPGetBulletins>($"EXECUTE uspSYS_GetBulletins").ToListAsync();
        //}

        // GET: api/SP/GetEmailAlias
        [HttpGet("GetEmailAlias")]
        public async Task<ActionResult<IEnumerable<SPGetEmailAlias>>> GetEmailAlias()
        {
            return await _context.Database.SqlQuery<SPGetEmailAlias>($"EXECUTE uspWEB_GetEmailAlias").ToListAsync();
        }

        // GET: api/SP/GetChairmen
        [HttpGet("GetChairmen/{NextYear}")]
        public async Task<ActionResult<IEnumerable<SPGetChairmen>>> GetChairmen(int NextYear = 0)
        {
            return await _context.Database.SqlQuery<SPGetChairmen>($"EXECUTE uspWEB_GetChairmen {NextYear}").ToListAsync();
        }

        // GET: api/SP/GetChairmanInfoBlock/{id}
        [HttpGet("GetChairmanInfoBlock/{id}/{NextYear}")]
        public async Task<ActionResult<IEnumerable<SPGetChairmanInfoBlock>>> GetChairmanInfoBlock(int id,int NextYear = 0)
        {
            //Check to see if we have a valid chairman id
            var myChairmenList = await _context.Database
                   .SqlQuery<SPGetChairmenId>($"EXECUTE uspWEB_IsChairman {id}")
                   .ToListAsync();

            if (myChairmenList.Count() == 0)
            {
                return NotFound();
            }
            return await _context.Database.SqlQuery<SPGetChairmanInfoBlock>($"EXECUTE uspWEB_GetChairmanInfoBlock {id},{NextYear}").ToListAsync();

        }

        // GET: api/SP/GetDDs
        [HttpGet("GetDDs/{NextYear}")]
        public async Task<ActionResult<IEnumerable<SPGetDDs>>> GetDDs(int NextYear = 0)
        {
            return await _context.Database.SqlQuery<SPGetDDs>($"EXECUTE uspWEB_GetDDs {NextYear}").ToListAsync();
        }

        // GET: api/SP/FourthDegreeOfficers
        [HttpGet("FourthDegreeOfficers")]
        public async Task<ActionResult<IEnumerable<SPGetChairmanInfoBlock>>> GetFourthDegreeOfficers()
        {
            return await _context.Database.SqlQuery<SPGetChairmanInfoBlock>($"EXECUTE uspWEB_GetFourthDegreeOfficers").ToListAsync();
        }
    }
}

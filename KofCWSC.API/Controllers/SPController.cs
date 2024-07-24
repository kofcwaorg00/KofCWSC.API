using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KofCWSC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SPController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public SPController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/SP/GetBulletins
        [HttpGet("GetBulletins")]
        public async Task<ActionResult<IEnumerable<SPGetBulletins>>> GetBulletins()
        {
            return await _context.Set<SPGetBulletins>().FromSqlRaw("EXECUTE uspSYS_GetBulletins").ToListAsync();
        }

        // GET: api/SP/GetEmailAlias
        [HttpGet("GetEmailAlias")]
        public async Task<ActionResult<IEnumerable<SPGetEmailAlias>>> GetEmailAlias()
        {
            return await _context.Set<SPGetEmailAlias>().FromSqlRaw("EXECUTE uspWEB_GetEmailAliasForRepeater").ToListAsync();
        }

        // GET: api/SP/GetChairmen
        [HttpGet("GetChairmen")]
        public async Task<ActionResult<IEnumerable<SPGetChairmen>>> GetChairmen()
        {
            return await _context.Set<SPGetChairmen>().FromSqlRaw("EXECUTE uspWEB_GetChairmen 0").ToListAsync();
        }

        // GET: api/SP/GetChairmanInfoBlock/{id}
        [HttpGet("GetChairmanInfoBlock/{id}")]
        public async Task<ActionResult<SPGetChairmanInfoBlock>> GetChairmanInfoBlock(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var result = await _context.Set<SPGetChairmanInfoBlock>().FromSqlRaw($"EXECUTE uspWEB_GetChairmanInfoBlock {id}").ToListAsync();

            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result.FirstOrDefault());
        }

        // GET: api/SP/GetDDs
        [HttpGet("GetDDs")]
        public async Task<ActionResult<IEnumerable<SPGetChairmanInfoBlock>>> GetDDs()
        {
            return await _context.Set<SPGetChairmanInfoBlock>().FromSqlRaw("EXECUTE uspWEB_GetDDs 0").ToListAsync();
        }

        // GET: api/SP/FourthDegreeOfficers
        [HttpGet("FourthDegreeOfficers")]
        public async Task<ActionResult<IEnumerable<SPGetChairmanInfoBlock>>> GetFourthDegreeOfficers()
        {
            return await _context.Set<SPGetChairmanInfoBlock>().FromSqlRaw("EXECUTE uspWEB_GetFourthDegreeOfficers").ToListAsync();
        }
    }
}

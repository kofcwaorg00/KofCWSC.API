using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Data;
using KofCWSC.API.Models;

namespace KofCWSC.API.Controllers
{
    [Route("")]
    [ApiController]
    public class SelfPublishController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public SelfPublishController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/SelfPublish
        [HttpGet("SelfPubs")]
        public async Task<ActionResult<IEnumerable<TblWebSelfPublish>>> GetAll()
        {
            return await _context.TblWebSelfPublishes.ToListAsync();
        }

        // GET: api/SelfPublish/Details/{id}
        [HttpGet("SelfPub/{id}")]
        public async Task<ActionResult<TblWebSelfPublish>> GetDetails(string id)
        {
            var tblWebSelfPublish = await _context.TblWebSelfPublishes
                .FirstOrDefaultAsync(m => m.Url == id);

            if (tblWebSelfPublish == null)
            {
                return NotFound();
            }

            return tblWebSelfPublish;
        }

        // GET: api/SelfPublish/Display/{id}
        [HttpGet("SelfPub/Display/{id}")]
        public async Task<ActionResult<IEnumerable<SPGetSOS>>> Display(int id)
        {
            var result = await _context.Database.SqlQuery<SPGetSOS>($"uspWEB_GetSOS {id}").ToListAsync();


            //var result = await _context.Set<SPGetSOS>()
            //    .FromSqlRaw($"uspWEB_GetSOS {id}")
            //    .ToListAsync();
            return new JsonResult(result);
        }

        // POST: api/SelfPublish/Create
        [HttpPost("SelfPub")]
        public async Task<ActionResult<TblWebSelfPublish>> Create([FromBody] TblWebSelfPublish tblWebSelfPublish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblWebSelfPublish);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetDetails", new { id = tblWebSelfPublish.Url }, tblWebSelfPublish);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/SelfPublish/Edit/{id}
        [HttpPut("SelfPub/{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody] TblWebSelfPublish tblWebSelfPublish)
        {
            if (id != tblWebSelfPublish.Url)
            {
                return BadRequest();
            }

            _context.Entry(tblWebSelfPublish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblWebSelfPublishExists(tblWebSelfPublish.Url))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/SelfPublish/Delete/{id}
        [HttpDelete("SelfPub/{id}")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tblWebSelfPublish = await _context.TblWebSelfPublishes.FindAsync(id);
            if (tblWebSelfPublish == null)
            {
                return NotFound();
            }

            _context.TblWebSelfPublishes.Remove(tblWebSelfPublish);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool TblWebSelfPublishExists(string id)
        {
            return _context.TblWebSelfPublishes.Any(e => e.Url == id);
        }
    }
}

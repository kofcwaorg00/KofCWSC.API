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
    [Route("api/[controller]")]
    [ApiController]
    public class SelfPublishController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public SelfPublishController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/TblWebSelfPublishes
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var items = await _context.TblWebSelfPublishes.ToListAsync();
            return Ok(items);
        }

        // GET: api/TblWebSelfPublishes/Details/{id}
        [HttpGet("Details/{id}")]
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

        /// <summary>
        /// changed
        /// Added Display to support the "Read More.." on in the carosel.  TblWebSelfPublishs does not provide additional data that 
        /// is used in the display of the message
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Display/{id}")]
        public async Task<ActionResult<IEnumerable<SPGetSOS>>> Display(int id)
        {
            return await _context.Set<SPGetSOS>().FromSql($"EXECUTE uspWEB_GetSOS {id}").ToListAsync();
        }

        // POST: api/TblWebSelfPublishes/Create
        [HttpPost("Create")]
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

        // PUT: api/TblWebSelfPublishes/Edit/{id}
        [HttpPut("Edit/{id}")]
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

        // DELETE: api/TblWebSelfPublishes/Delete/{id}
        [HttpDelete("Delete/{id}")]
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

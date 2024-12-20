﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Data;
using KofCWSC.API.Models;
using NuGet.Protocol;

namespace KofCWSC.API.Controllers
{
    [Route("")]
    [ApiController]
    public class CouncilsController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public CouncilsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/TblValCouncils
        [HttpGet("Councils")]
        public async Task<ActionResult<IEnumerable<TblValCouncil>>> GetTblValCouncils()
        {
            return await _context.TblValCouncils
                .Where(x => x.CNumber > 0)
                .OrderBy(x => x.CNumber)
                .ToListAsync();
        }

        // GET: api/TblValCouncils/5
        [HttpGet("Council/{id}")]
        public async Task<ActionResult<TblValCouncil>> GetTblValCouncil(int id)
        {
            var tblValCouncil = await _context.TblValCouncils
                .FirstOrDefaultAsync(m => m.CNumber == id);
            if (tblValCouncil == null)
            {
                return NotFound();
            }
            return Ok(tblValCouncil);
        }

        // POST: api/TblValCouncils
        [HttpPost("Council")]
        public async Task<ActionResult<TblValCouncil>> CreateTblValCouncil(TblValCouncil tblValCouncil)
        {
            _context.TblValCouncils.Add(tblValCouncil);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTblValCouncil), new { id = tblValCouncil.CNumber }, tblValCouncil);
        }

        // PUT: api/TblValCouncils/5
        [HttpPut("Council/{id}")]
        public async Task<ActionResult<TblValCouncil>> UpdateTblValCouncil(int id, TblValCouncil tblValCouncil)
        {
            if (id != tblValCouncil.CNumber)
            {
                // not sure if this would ever happen but we need to communicate back
                // to the calling process what happened
                return BadRequest("Council numbers mismatch");
            }
            _context.Entry(tblValCouncil).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblValCouncilExists(id))
                {
                    return BadRequest("Concurrency Issue");
                }
                else
                {
                    return BadRequest("Unknown");
                }
            }

            return Ok(tblValCouncil);
        }

        // DELETE: api/TblValCouncils/5
        [HttpDelete("Council/{id}")]
        public async Task<IActionResult> DeleteTblValCouncil(int id)
        {
            var tblValCouncil = await _context.TblValCouncils.FindAsync(id);
            if (tblValCouncil == null)
            {
                return NotFound();
            }

            _context.TblValCouncils.Remove(tblValCouncil);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool TblValCouncilExists(int id)
        {
            return _context.TblValCouncils.Any(e => e.CNumber == id);
        }
    }
}


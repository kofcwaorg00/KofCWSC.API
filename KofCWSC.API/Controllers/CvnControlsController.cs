using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace KofCWSC.API.Controllers
{
    public class CvnControlsController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public CvnControlsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }


        // GET: CvnControls/Edit/5
        [HttpGet("Control/{id}")]
        public async Task<ActionResult<CvnControl>> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var cvnControl = await _context.TblCvnControls.FindAsync(id);
                if (cvnControl == null)
                {
                    return NotFound();
                }
                else
                {
                    return cvnControl;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Helper.FormatLogEntry(this, ex));
                return BadRequest(ex);
            }
            
        }

        // POST: CvnControls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("Control/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PostControl(int id,[FromBody] CvnControl cvnControl)
        {
            if (id != cvnControl.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cvnControl);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CvnControlExists(cvnControl.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        Log.Error("Update of Control Data Error");
                        return BadRequest();
                    }
                }
                return Ok();
            }
            return Ok();
        }

        private bool CvnControlExists(int id)
        {
            return _context.TblCvnControls.Any(e => e.Id == id);
        }
    }
}

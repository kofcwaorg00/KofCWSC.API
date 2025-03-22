using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Data;
using KofCWSC.API.Models;
using KofCWSC.API.Utils;
using Serilog;
using NuGet.LibraryModel;


namespace KofCWSCWebsite.Controllers
{
    public class CvnImpDelegatesController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public CvnImpDelegatesController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        [HttpGet("CvnImpDelegatesIMP")]
        public async Task<ActionResult<IEnumerable<CvnImpDelegateIMP>>> IndexIMP()
        {
            var results = await _context.CvnImpDelegateIMPs.ToListAsync();
            return results;
        }

        [HttpGet("CvnImpDelegates")]
        // GET: CvnImpDelegates
        public async Task<ActionResult<IEnumerable<CvnImpDelegate>>> Index()
        {
            var results = await _context.Database
                    .SqlQuery<CvnImpDelegate>($"EXECUTE uspCVN_GetImpDelegates")
                    .ToListAsync();

            return results;
            //return View(await _context.CvnImpDelegates.ToListAsync());
        }

        //// GET: CvnImpDelegates/Details/5
        //[HttpGet("CvnImpDelegates/{id}")]
        //public async Task<ActionResult<CvnImpDelegate>> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cvnImpDelegate = await _context.CvnImpDelegates
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (cvnImpDelegate == null)
        //    {
        //        return NotFound();
        //    }

        //    return cvnImpDelegate;
        //}


        // POST: CvnImpDelegates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("CvnImpDelegate")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<CvnImpDelegateIMP>> Create([Bind("SubmissionDate,FormSubmitterSEmail,CouncilName,CouncilNumber,D1FirstName,D1MiddleName,D1LastName,D1Suffix,D1MemberID,D1Address1,D1Address2,D1City,D1State,D1ZipCode,D1Phone,D1Email,D2FirstName,D2MiddleName,D2LastName,D2Suffix,D2MemberID,D2Address1,D2Address2,D2City,D2State,D2ZipCode,D2Phone,D2Email,A1FirstName,A1MiddleName,A1LastName,A1Suffix,A1MemberID,A1Address1,A1Address2,A1City,A1State,A1ZipCode,A1Phone,A1Email,A2FirstName,A2MiddleName,A2LastName,A2Suffix,A2MemberID,A2Address1,A2Address2,A2City,A2State,A2ZipCode,A2Phone,A2Email,Id,RecType")] [FromBody] CvnImpDelegateIMP cvnImpDelegateIMP)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(cvnImpDelegateIMP);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.Message.ToLower().Contains("duplicate key"))
                    {
                        return Json("Duplicate Key Detected");
                        //return Ok("Duplicate Key Detected");
                    }
                    if (ex.InnerException.Message.ToLower().Contains("foreign key"))
                    {
                        return Json("Council not found");
                    }
                }
                
            }
            return BadRequest("Model is invalid");
        }

        [HttpGet("CvnImpDelegateIMP/{id}")]
        public async Task<ActionResult<CvnImpDelegate>> GetByID(int? id)
        {
            var results = _context.Database
                       .SqlQuery<CvnImpDelegate>($"EXECUTE uspCVN_GetImpDelegatesByID {id}")
                       .AsEnumerable()
                       .FirstOrDefault();
            return results;
        }
        // GET: CvnImpDelegates/Edit/5
        [HttpGet("CvnImpDelegate/{id}")]
        public async Task<ActionResult<CvnImpDelegateIMP>> Edit(int? id)
        {
            try
            {
                var results = await _context.CvnImpDelegateIMPs
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (results == null)
                {
                    return NotFound();
                }
                return Ok(results);
            }
            catch (Exception ex)
            {
                Log.Error(Helper.FormatLogEntry(this, ex));
                return NotFound();
            }
            

        }

        // POST: CvnImpDelegates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("CvnImpDelegate/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] CvnImpDelegateIMP cvnImpDelegateIMP)
        {
            if (id != cvnImpDelegateIMP.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cvnImpDelegateIMP);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CvnImpDelegateExists(cvnImpDelegateIMP.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
                return Ok(Json("Success"));
            }
            return Ok();
        }


        // POST: CvnImpDelegates/Delete/5
        [HttpDelete("CvnImpDelegate/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cvnImpDelegateIMP = await _context.CvnImpDelegateIMPs.FindAsync(id);
            if (cvnImpDelegateIMP != null)
            {
                _context.CvnImpDelegateIMPs.Remove(cvnImpDelegateIMP);
            }

            await _context.SaveChangesAsync();
            return Ok();   
        }

        private bool CvnImpDelegateExists(int id)
        {
            return _context.CvnImpDelegateIMPs.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using KofCWSC.API.Data;
using KofCWSC.API.Models;

namespace KofCWSCWebsite.Controllers
{
    public class EmailOfficesController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public EmailOfficesController(KofCWSCAPIDBContext context, IConfiguration configuration)
        {
            _context = context;
        }

        // GET: EmailOffices
        [HttpGet("Emails")]
        public async Task<ActionResult<IEnumerable<EmailOffice>>> GetEmails()
        {
            try
            {
                return await _context.TblWebTrxEmailOffices
                    .OrderByDescending(d => d.DateSent)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + " - " + ex.InnerException);
                return NotFound();
            }

        }

        // GET: EmailOffices/5
        [HttpGet("Emails/{id}")]
        public async Task<ActionResult<EmailOffice>> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailOffice = await _context.TblWebTrxEmailOffices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emailOffice == null)
            {
                return NotFound();
            }

            return emailOffice;
        }

        // POST: EmailOffices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Emails")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<EmailOffice>> PostEmailOffice([FromBody]EmailOffice emailOffice)
        {
            try
            {
                _context.TblWebTrxEmailOffices.Add(emailOffice);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    status = "success",
                    message = "Email office saved successfully"
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Error saving email office: {ex.Message}", ex);

                return Json(new
                {
                    status = "error",
                    message = "An error occurred while saving the email office",
                    details = ex.InnerException?.Message
                });
            }
            //try
            //{
            //    _context.TblWebTrxEmailOffices.Add(emailOffice);
            //    await _context.SaveChangesAsync();
            //    return Json("Success");
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex.Message + " - " + ex.InnerException);
            //    return Json("Error");
            //}
        }
    }
}

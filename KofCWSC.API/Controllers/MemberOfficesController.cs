﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Models;
using KofCWSC.API.Data;
using Serilog;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.Metadata;

namespace KofCWSC.API.Controllers
{
    [Route("")]
    [ApiController]
    public class MemberOfficesController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public MemberOfficesController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: CheckForMissingDelegateMembersAndCreateDelegates
        [HttpGet("CheckForMissingDelegateMembersAndCreateDelegates")]
        public async Task<ActionResult<IEnumerable<TblCorrMemberOfficeVM>>> CheckForMissingDelegateMembersAndCreateDelegates()
        {
            //******************************************************************************************************************
            //  1/25/2025 Tim Philomeno
            //  this will return either those imported records that do not have a correspoinding
            //  member record or an empty result set if all imported delegates have a member record
            //  then this sproc will populate the corrmemberoffice table with the new delegates and return an empty result set
            //  we can test the model in our controller and can't finish until the missing members
            //  have been resolved
            //-----------------------------------------------------------------------------------------------------------------
            try
            {
                var results = await _context.Database
                    .SqlQuery<TblCorrMemberOfficeVM>($"EXECUTE uspCVN_CheckForMissingDelegateMembersAndCreateDelegates")
                    .ToListAsync();

                return results;
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Helper.FormatLogEntry(this,ex));
                return BadRequest(ex.InnerException);
            }    
            

        }
        // GET: api/MemberOffices/{id}
        [HttpGet("MemberOffice/{id}")]
        public async Task<ActionResult<IEnumerable<TblCorrMemberOfficeVM>>> GetMemberOffices(int id)
        {
            return await _context.Database
                    .SqlQuery<TblCorrMemberOfficeVM>($"EXECUTE uspSYS_GetOfficesForMemberID {id}")
                    .ToListAsync();
            
        }

        // GET: api/MemberOffices/Details/5
        [HttpGet("MemberOffice/Details/{id}")]
        public async Task<ActionResult<TblCorrMemberOffice>> GetMemberOfficeDetails(int id)
        {
            try
            {
                var memberOffice = await _context.TblCorrMemberOffices.FindAsync(id);
                if (memberOffice == null)
                {
                    return NotFound();
                }
                return memberOffice;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + "-" + ex.StackTrace);
                return NotFound();  
            }
        }

        // POST: api/MemberOffices
        [HttpPost("MemberOffice")]
        public async Task<ActionResult<TblCorrMemberOffice>> CreateMemberOffice([FromBody] TblCorrMemberOffice memberOffice)
        {
            _context.TblCorrMemberOffices.Add(memberOffice);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMemberOfficeDetails), new { id = memberOffice.Id }, memberOffice);
        }

        // PUT: api/MemberOffices/5
        [HttpPut("MemberOffice/{id}")]
        public async Task<IActionResult> UpdateMemberOffice(int id, [FromBody] TblCorrMemberOffice memberOffice)
        {
            if (id != memberOffice.Id)
            {
                return BadRequest();
            }

            _context.Entry(memberOffice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TblCorrMemberOffices.Any(e => e.Id == id))
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

        // DELETE: api/MemberOffices/5
        [HttpDelete("MemberOffice/{id}")]
        public async Task<IActionResult> DeleteMemberOffice(int id)
        {
            var memberOffice = await _context.TblCorrMemberOffices.FindAsync(id);
            if (memberOffice == null)
            {
                return NotFound();
            }

            _context.TblCorrMemberOffices.Remove(memberOffice);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
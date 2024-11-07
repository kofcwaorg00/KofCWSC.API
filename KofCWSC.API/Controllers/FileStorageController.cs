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
using Microsoft.AspNetCore.Diagnostics;


namespace KofCWSC.API.Controllers
{
    public class FileStorageController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public FileStorageController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }


        [HttpPost("Files")]
        public async Task<IActionResult> Upload([FromBody] FileStorage fileStorage)
        {
            try
            {
                _context.FileStorages.Add(fileStorage);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + " - " + ex.InnerException);
                return BadRequest();
            }

            return Json(fileStorage); ;
        }

        // GET: TblWebFileStorages
        [HttpGet("Files")]
        public async Task<ActionResult<IEnumerable<FileStorageVM>>> Index()
        {
            //*****************************************************************************
            // 9/21/2024 Tim Philomeno
            // execute a select statement so we don't get the DATA stream, takes too long
            //-----------------------------------------------------------------------------
            return await _context.Database.SqlQuery<FileStorageVM>($"EXECUTE uspWEB_GetFileStorageVM").ToListAsync();
        }

        // GET: TblWebFileStorages/Details/5
        [HttpGet("Files/{id}")]
        public async Task<ActionResult<FileStorage>> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var fileStorage = await _context.FileStorages
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (fileStorage == null)
                {
                    return NotFound();
                }
                return fileStorage;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + " - " + ex.InnerException);
                return NotFound();
            }


        }


        //*****************************************************************************
        // 11/7/2024 Tim Philomeno
        // GetPDF supports the Ajax call in FileStorage Details
        [HttpPost("Files/{id}")]
        public JsonResult GetPDF(int? id)
        {
            if (id == null)
            {
                return Json(NotFound());
            }
            var tblWebFileStorage = _context.Database.SqlQuery<FileStorage>($"SELECT * FROM tblWEB_FileStorage where Id={id}").ToList();
            var tfs = tblWebFileStorage.FirstOrDefault();
            if (tblWebFileStorage == null)
            {
                return Json(NotFound());
            }

            return Json(new { FileName = tfs.FileName, ContentType = tfs.ContentType, Data = tfs.Data });
        }

        // POST: TblWebFileStorages/Delete/5
        [HttpDelete("Files/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var tblWebFileStorage = await _context.FileStorages.FindAsync(id);
            if (tblWebFileStorage != null)
            {
                _context.FileStorages.Remove(tblWebFileStorage);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

using KofCWSC.API.Data;
using KofCWSC.API.Models;
using KofCWSC.API.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Microsoft.Data.SqlClient;

namespace KofCWSC.API.Controllers
{
    public class ConventionController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;
        public ConventionController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: CvnControls/Edit/5
        [HttpGet("ImpDelegatesLog/{GUID}")]
        public async Task<ActionResult<IEnumerable<CvnImpDelegatesLog>>> GetDelegatesLog(Guid GUID)
        {
            return await _context.TblCvnImpDelegatesLogs.Where(p => p.Guid == GUID).ToListAsync();
        }
        // GET: CvnControls/Edit/5
        [HttpGet("ImpDelegates")]
        public async Task<ActionResult<IEnumerable<CvnImpDelegate>>> GetDelegates()
        {
            return await _context.CvnImpDelegates.ToListAsync();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("ImpDelegates")]
        public async Task<ActionResult<CvnImpDelegate>> ImpDelegates([FromBody] List<CvnImpDelegate> cvnImpDelegate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // first delete the existing table data
                    _context.Database.ExecuteSqlRaw("TRUNCATE TABLE [tblCVN_ImpDelegates]");
                    // second import the incoming data
                    foreach (var myDel in cvnImpDelegate)
                    {
                        _context.CvnImpDelegates.Add(myDel);
                        //ProcessCouncil(myDel);
                    }

                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest("Model State is Invalid");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + " - " + ex.InnerException);
                return BadRequest($"Error saving changes - {ex.Message}");
            }
            //return Ok("Import Successful");
            return Json("Import Successful");
        }

        // GET: CvnControls/Edit/5
        [HttpGet("GetAttendeeDays")]
        public async Task<ActionResult<IEnumerable<CvnDelegateDays>>> GetDelegateDays()
        {
            return await _context.Database.SqlQuery<CvnDelegateDays>($"EXECUTE uspCVN_GetAttendeeDays").ToListAsync();
        }

        [HttpGet("ToggleDelegateDays/{id}/{day}")]
        public int ToggleDelegateDays(int id,int day)
        {
            return  _context.Database.ExecuteSql($"EXECUTE [uspCVN_ToggleSeatedDays] {id}, {day}");
        }
        [HttpGet("ToggleCouncilDays/{id}/{day}/{del}")]
        public int ToggleCouncilDays(int id, int day,string del)
        {
            return _context.Database.ExecuteSql($"EXECUTE [uspCVN_ToggleCouncilDays] {id}, {day},{del}");
        }
        [HttpGet("ResetDelegates")]
        public int ResetDelegates()
        {
            return _context.Database.ExecuteSql($"EXECUTE [uspCVN_ResetDelegates]");
        }


        // REPORTS
        [HttpGet("/PrintMPDChecks/{GroupID}/{BegChkNbr}/{PrintCheckNumber}")]
        public IEnumerable<PrintMPDChecks> GetLabelByGroup(int GroupID,int BegChkNbr,int PrintCheckNumber)
        {
            if (!(GroupID == 3 || GroupID == 25))
            {
                var _new = new List<PrintMPDChecks>();
                return _new;
            }
            try
            {
                return _context.Database
                .SqlQuery<PrintMPDChecks>($"[uspCVN_PrintMPDChecks] {GroupID},{BegChkNbr},{PrintCheckNumber}")
                .ToList();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message + " " + ex.InnerException);
                var _new = new List<PrintMPDChecks>();
                return _new;
            }
        }


        //[HttpGet("ProcessDelegateImport/{GUID}")]
        //public async Task<ActionResult> ProcessDelegateImport(Guid GUID)
        //{

        //    // third run the porcessing sproc
        //    //_context.Database.ExecuteSqlRaw("EXECUTE uspCVN_ProcessDelegates");
        //    //*****************************************************************************************************
        //    // 11/28/2024 Tim Philomeno
        //    // added this Task.Run so I don't have to wait for the processing of the delegates on the server
        //    // otherwise the process will timeout
        //    // NOTE: see the tblCVN_ImpDelegatesLog for the results...
        //    //Task.Run(() => ExecuteStoredProcAsyns(GUID));
        //    try
        //    {
        //        //string sql = "EXECUTE uspCVN_ProcessDelegates @GUID";

        //        //var parameters = new[]
        //        //{
        //        //    new SqlParameter("@GUID", GUID)
        //        //};

        //        //await _context.Database.ExecuteSqlRawAsync(sql, parameters);

        //        string sql = $"EXECUTE uspCVN_ProcessDelegates " + "{" + GUID + "}" + "";
        //        await _context.Database.ExecuteSqlRawAsync(sql);


        //       // await _context.Database.ExecuteSqlRawAsync($"EXECUTE uspCVN_ProcessDelegates '" + GUID + "'");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(Helper.FormatLogEntry(this, ex));
        //        return BadRequest(Helper.FormatLogEntry(this, ex));
        //    }


        //    return Ok();

        //}
        //[HttpPost("CreateLogEntry")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([FromBody] CvnImpDelegatesLog cvnImpDelegatesLog)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(cvnImpDelegatesLog);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(cvnImpDelegatesLog);
        //}

        //private async Task ExecuteStoredProcAsyns(Guid GUID)
        //{
        //    try
        //    {
        //        await _context.Database.ExecuteSqlRawAsync($"EXECUTE uspCVN_ProcessDelegates {GUID}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message + " " + ex.InnerException);
        //        throw;
        //    }

        //}

    }
}

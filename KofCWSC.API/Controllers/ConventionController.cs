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
using Microsoft.IdentityModel.Tokens;

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
        public async Task<ActionResult<IEnumerable<CvnImpDelegateIMP>>> GetDelegates()
        {
            return await _context.CvnImpDelegateIMPs.ToListAsync();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("ImpDelegates")]
        public async Task<ActionResult<CvnImpDelegateIMP>> ImpDelegates([FromBody] List<CvnImpDelegateIMP> cvnImpDelegate)
        {
            string myRetMess = "";
            try
            {
                
                if (ModelState.IsValid)
                {
                    // first delete the existing table data
                    //_context.Database.ExecuteSqlRaw("TRUNCATE TABLE [tblCVN_ImpDelegates]");
                    // second import the incoming data
                    foreach (var myDel in cvnImpDelegate)
                    {
                        try
                        {
                            //----------------------------------------------------------------------------------------
                            // D1
                            myDel.D1FirstName = Helper.CUpLow(myDel.D1FirstName);
                            myDel.D1LastName = Helper.CUpLow(myDel.D1LastName);
                            myDel.D1Phone = Helper.FormatPhoneNumber(myDel.D1Phone);
                            if (!myDel.D1Address1.IsNullOrEmpty()) { myDel.D1Address1 = myDel.D1Address1.ToUpper(); }
                            if (!myDel.D1Address2.IsNullOrEmpty()) { myDel.D1Address2 = myDel.D1Address2.ToUpper(); }
                            if (!myDel.D1City.IsNullOrEmpty()) { myDel.D1City = myDel.D1City.ToUpper(); }
                            if (!myDel.D1State.IsNullOrEmpty()) { myDel.D1State = Helper.GetStateAbbr(myDel.D1State); }
                            if (!myDel.D1Email.IsNullOrEmpty()) { myDel.D1Email = myDel.D1Email.ToUpper(); }
                            //----------------------------------------------------------------------------------------
                            // D2
                            myDel.D2FirstName = Helper.CUpLow(myDel.D2FirstName);
                            myDel.D2LastName = Helper.CUpLow(myDel.D2LastName);
                            myDel.D2Phone = Helper.FormatPhoneNumber(myDel.D2Phone);
                            if (!myDel.D2Address1.IsNullOrEmpty()) { myDel.D2Address1 = myDel.D2Address1.ToUpper(); }
                            if (!myDel.D2Address2.IsNullOrEmpty()) { myDel.D2Address2 = myDel.D2Address2.ToUpper(); }
                            if (!myDel.D2City.IsNullOrEmpty()) { myDel.D2City = myDel.D2City.ToUpper(); }
                            if (!myDel.D2State.IsNullOrEmpty()) { myDel.D2State = Helper.GetStateAbbr(myDel.D2State); }
                            if (!myDel.D2Email.IsNullOrEmpty()) { myDel.D2Email = myDel.D2Email.ToUpper(); }
                            //----------------------------------------------------------------------------------------
                            // A1
                            myDel.A1FirstName = Helper.CUpLow(myDel.A1FirstName);
                            myDel.A1LastName = Helper.CUpLow(myDel.A1LastName);
                            myDel.A1Phone = Helper.FormatPhoneNumber(myDel.A1Phone);
                            if (!myDel.A1Address1.IsNullOrEmpty()) { myDel.A1Address1 = myDel.A1Address1.ToUpper(); }
                            if (!myDel.A1Address2.IsNullOrEmpty()) { myDel.A1Address2 = myDel.A1Address2.ToUpper(); }
                            if (!myDel.A1City.IsNullOrEmpty()) { myDel.A1City = myDel.A1City.ToUpper(); }
                            if (!myDel.A1State.IsNullOrEmpty()) { myDel.A1State = Helper.GetStateAbbr(myDel.A1State); }
                            if (!myDel.A1Email.IsNullOrEmpty()) { myDel.A1Email = myDel.A1Email.ToUpper(); }
                            //----------------------------------------------------------------------------------------
                            // A2
                            myDel.A2FirstName = Helper.CUpLow(myDel.A2FirstName);
                            myDel.A2LastName = Helper.CUpLow(myDel.A2LastName);
                            myDel.A2Phone = Helper.FormatPhoneNumber(myDel.A2Phone);
                            if (!myDel.A2Address1.IsNullOrEmpty()) { myDel.A2Address1 = myDel.A2Address1.ToUpper(); }
                            if (!myDel.A2Address2.IsNullOrEmpty()) { myDel.A2Address2 = myDel.A2Address2.ToUpper(); }
                            if (!myDel.A2City.IsNullOrEmpty()) { myDel.A2City = myDel.A2City.ToUpper(); }
                            if (!myDel.A2State.IsNullOrEmpty()) { myDel.A2State = Helper.GetStateAbbr(myDel.A2State); }
                            if (!myDel.A2Email.IsNullOrEmpty()) { myDel.A2Email = myDel.A2Email.ToUpper(); }
                            //----------------------------------------------------------------------------------------
                            // next line should clear context so the previous adds are gone
                            _context.ChangeTracker.Clear();
                            
                            _context.CvnImpDelegateIMPs.Add(myDel);
                            await _context.SaveChangesAsync();

                        }
                        catch (DbUpdateException ex)
                        {
                            if (ex.InnerException.Message.Contains("duplicate key"))
                            {
                                myRetMess += $"Skipping Duplicate Council {myDel.CouncilNumber}; ";
                                // do nothing just ignore
                            }
                            else
                            {
                                myRetMess += $"Skipping Missing Council {myDel.CouncilNumber}; ";
                                // do something
                            }
                        }
                    }
                }
                else
                {
                    return BadRequest(myRetMess);
                }
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex.Message + " - " + ex.InnerException);
                return BadRequest($"Error saving changes - {ex.Message}");
            }
            //return Ok("Import Successful");
            return Json(myRetMess);
        }

        // GET: CvnControls/Edit/5
        [HttpGet("GetAttendeeDays")]
        public async Task<ActionResult<IEnumerable<CvnDelegateDays>>> GetDelegateDays()
        {
            return await _context.Database.SqlQuery<CvnDelegateDays>($"EXECUTE uspCVN_GetAttendeeDays").ToListAsync();
        }

        [HttpGet("ToggleDelegateDays/{id}/{day}")]
        public int ToggleDelegateDays(int id, int day)
        {
            return _context.Database.ExecuteSql($"EXECUTE [uspCVN_ToggleSeatedDays] {id}, {day}");
        }
        [HttpGet("ToggleCouncilDays/{id}/{day}/{del}")]
        public int ToggleCouncilDays(int id, int day, string del)
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
        public IEnumerable<PrintMPDChecks> GetLabelByGroup(int GroupID, int BegChkNbr, int PrintCheckNumber)
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
        private static void FormatMemberDataToSpec(ref CvnImpDelegate cvnImpDelegate)
        {
            cvnImpDelegate.D1Phone = Helper.FormatPhoneNumber(cvnImpDelegate.D1Phone);
            if (!cvnImpDelegate.D1Address1.IsNullOrEmpty()) { cvnImpDelegate.D1Address1 = cvnImpDelegate.D1Address1.ToUpper(); }
            if (!cvnImpDelegate.D1Address2.IsNullOrEmpty()) { cvnImpDelegate.D1Address2 = cvnImpDelegate.D1Address2.ToUpper(); }
            if (!cvnImpDelegate.D1City.IsNullOrEmpty()) { cvnImpDelegate.D1City = cvnImpDelegate.D1City.ToUpper(); }
            if (!cvnImpDelegate.D1State.IsNullOrEmpty()) { cvnImpDelegate.D1State = cvnImpDelegate.D1State.ToUpper(); }
            if (!cvnImpDelegate.D1Email.IsNullOrEmpty()) { cvnImpDelegate.D1Email = cvnImpDelegate.D1Email.ToUpper(); }
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

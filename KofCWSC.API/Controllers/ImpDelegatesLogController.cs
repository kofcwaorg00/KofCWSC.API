using KofCWSC.API.Data;
using KofCWSC.API.Models;
using KofCWSC.API.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KofCWSC.API.Controllers
{
    public class ImpDelegatesLogController : Controller
    {

        private readonly KofCWSCAPIDBContext _context;
        public ImpDelegatesLogController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: ImpDelegatesLog
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet("GetImpDelegatesLog/{guid}")]
        public async Task<ActionResult<IEnumerable<CvnImpDelegatesLog>>> GetImpDelegatesLog(Guid guid)
        {
            return await _context.TblCvnImpDelegatesLogs
                .Where(x => x.Guid == guid)
                .ToListAsync();
        }
        [HttpGet("ClearDelegates/{year}")]
        public int ClearDelegates(int year)
        {
            try
            {
                var RetVal = _context.Database.ExecuteSql($"DELETE FROM tbl_CorrMemberOffice where OfficeID IN(115,116,118,119) and year = {year}");
                return RetVal;
            }
            catch (Exception ex)
            {
                Log.Error(Helper.FormatLogEntry(this, ex));
            }
            return 0;
        }


        // GET: ImpDelegatesLog/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ImpDelegatesLog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImpDelegatesLog/Create
        [HttpPost("CreateImpDelegatesLog")]
        public async Task<ActionResult<CvnImpDelegatesLog>> CreateImpDelegatesLog([FromBody] CvnImpDelegatesLog cvnImpDelegatesLog)
        {
            _context.TblCvnImpDelegatesLogs.Add(cvnImpDelegatesLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateImpDelegatesLog), new { id = cvnImpDelegatesLog.Id }, cvnImpDelegatesLog);
        }

        // GET: ImpDelegatesLog/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ImpDelegatesLog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ImpDelegatesLog/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ImpDelegatesLog/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

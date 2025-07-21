using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace KofCWSC.API.Controllers
{
    public class EmailGroupsController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;
        public EmailGroupsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: api/SP/GetDDs
        [HttpGet("GetDDL/{GroupID}/{NextYear}")]
        public async Task<ActionResult<IEnumerable<EmailGroups>>> GetDDL(int GroupID, int NextYear = 0)
        {
            return await _context.Database.SqlQuery<EmailGroups>($"EXECUTE uspWEB_GetDDL {GroupID}, {NextYear}").ToListAsync();
        }
        // GET: api/SP/GetDDs
        [HttpGet("GetDistListForExchange/{Type}/{GroupID}/{OfficeID}/{NextYear}")]
        public async Task<ActionResult<IEnumerable<DistListForExchange>>> GetDistListForExchange(string Type, int GroupID, int OfficeID, int NextYear = 0)
        {
            try
            {
                var results = await _context.Database.SqlQuery<DistListForExchange>($"EXECUTE uspSYS_GetDistListForExchange {Type},{GroupID},{OfficeID}, {NextYear}").ToListAsync();
                // if there are DDs we need to add the missing districts
                if (GroupID == 3)
                {
                    if (Type == "AGWM")
                    {
                        // add in the missing dds
                        var missingdds = await _context.Database.SqlQuery<int>($"EXECUTE uspSYS_GetMissingDDs {NextYear}").ToListAsync();
                        // spin through and add them to results
                        foreach (var dd in missingdds)
                        {
                            if (!(dd.ToString() == "0"))
                            {
                                var newdd = new DistListForExchange
                                {
                                    GroupName = $"DD{dd.ToString("D2")}",
                                    GroupEmail = $"DD{dd.ToString("D2")}@kofc-wa.org",
                                    RecipientName = $"District Deputy Director",
                                    RecipientEmail = $"DDD@kofc-wa.org",
                                    ManagedBy = results.FirstOrDefault().ManagedBy,
                                    Alias = $"DD{dd.ToString()}"
                                };
                                results.Add(newdd);
                            }
                        }
                    }
                }
                if (Type == "AGWG")
                {
                    // add in the missing dds
                    var missingdds = await _context.Database.SqlQuery<int>($"EXECUTE uspSYS_GetMissingDDs {NextYear}").ToListAsync();
                    // spin through and add them to results
                    foreach (var dd in missingdds)
                    {
                        var newdd = new DistListForExchange
                        {
                            GroupName = $"ALL District Deputies",
                            GroupEmail = $"ALLDDs@kofc-wa.org",
                            RecipientName = $"District Deputy Director",
                            RecipientEmail = $"DDD@kofc-wa.org",
                            ManagedBy = results.FirstOrDefault().ManagedBy,
                            Alias = $"DD{dd.ToString()}"
                        };
                        results.Add(newdd);
                    }
                }
                return results; ;
        }
            catch (Exception ex)
            {
                Utils.Helper.FormatLogEntry(this, ex);
                return BadRequest();
            }

}

    }
}

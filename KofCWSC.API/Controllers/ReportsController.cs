using KofCWSC.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KofCWSC.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public ReportsController(KofCWSCAPIDBContext context)
        {
            Log.Information("Initialize DB Context");
            _context = context;
        }

        [HttpGet("/GetLabelByOffice/{OfficeID}")]
        public IEnumerable<GetLabelByOffice> GetLabelByOffice(int OfficeID)
        {
            Log.Information($"Starting GetLabelByOffice {OfficeID}");
            if (OfficeID == 0)
            {
                OfficeID = 1;
            }
            try
            {
                return _context.Database
                .SqlQuery<GetLabelByOffice>($"uspRPT_GetLabelByOfficeFR {OfficeID}")
                .ToList();

            }
            catch (Exception ex)
            {

                Log.Fatal(ex.Message + " " + ex.InnerException);
                var _new = new List<GetLabelByOffice>();
                _new = null;
                return _new;

            }

        }
    }
}

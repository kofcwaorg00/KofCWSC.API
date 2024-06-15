using KofCWSC.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KofCWSC.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly KofCWSCAPIDBContext _context;

        public ReportsController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        [HttpGet("/GetLabelByOffice/{OfficeID}")]
        public IEnumerable<GetLabelByOffice> GetLabelByOffice(int OfficeID)
        {

            if (OfficeID == 0)
            {
                OfficeID = 1;
            }

            return _context.Database
               .SqlQuery<GetLabelByOffice>($"uspRPT_GetLabelByOfficeFR {OfficeID}")
               .ToList();

        }
    }
}

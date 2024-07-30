using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace KofCWSC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KofCWSCAPIDBContext _context;

        public HomeController(KofCWSCAPIDBContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Home
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string environment;
            var connectionString = _context.Database.GetDbConnection().ConnectionString;

            if (connectionString.Contains("2k2201"))
            {
                environment = "Using DASP DEVELOPMENT DATABASE";
            }
            else if (connectionString.Contains("KofCWSCWebDEV"))
            {
                environment = "Using AZURE DEVELOPMENT DATABASE";
            }
            else if (connectionString.Contains("KofCWSCWeb"))
            {
                environment = "Using AZURE PRODUCTION DATABASE";
            }
            else
            {
                environment = "Using DASP PRODUCTION DATABASE";
            }

            var result = await _context.Set<HomePageViewModel>()
                .FromSqlRaw("EXECUTE uspWEB_GetHomePage")
                .ToListAsync();

            return Ok(new
            {
                Environment = environment,
                APIURL = "Using AZDEV",
                Result = result
            });
        }

        // GET: api/Home/Privacy
        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return Ok(new { Message = "Privacy information." });
        }

        // GET: api/Home/HealthCheck
        [HttpGet("HealthCheck")]
        public IActionResult HealthCheck()
        {
            return Ok(new { Status = "Healthy" });
        }

        // GET: api/Home/Error
        [HttpGet("Error")]
        public IActionResult Error()
        {
            return Problem(detail: "An error occurred.", instance: HttpContext.TraceIdentifier);
        }
    }
}

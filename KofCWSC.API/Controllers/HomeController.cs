using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KofCWSC.API.Controllers
{
    [Route("")]
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

        // GET: api/HomeEnv
        [HttpGet("HomeEnv")]
        public IActionResult GetEnv()
        {
            try
            {
                var myENV = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                return Ok(myENV);
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Helper.FormatLogEntry(this, ex));
                return BadRequest(ex.Message);
            }

            //return new JsonResult(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }

        // GET: api/Home
        [HttpGet("Home")]
        public async Task<IActionResult> Index()
        {
            var result = await _context.Database
                .SqlQuery<HomePageViewModel>($"uspWEB_GetHomePage")
                .ToListAsync();

            //return new JsonResult(result);
            return Ok(result);
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

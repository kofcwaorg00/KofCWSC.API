global using KofCWSC.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;



namespace KofCWSC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RosterController : ControllerBase
    {
        private static List<Roster> people = new List<Roster> { 
        new Roster()};
        // private readonly ILogger<RosterController> _logger;

        // public RosterController(ILogger<RosterController> logger)
        //{
        // _logger = logger;
        //  }

        [HttpGet(Name = "Roster")]
        // public async Task<ActionResult<List<Roster>>> GetAllRoster()
        public ActionResult<Roster> Get()
        {
         //var result = await _logger.Roster.ToListAsync();
         return Ok(people);
        }



    }

}
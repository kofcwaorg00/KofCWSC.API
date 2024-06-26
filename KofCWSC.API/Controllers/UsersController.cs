using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.RegularExpressions;

namespace KofCWSC.API.Controllers
{
    //[Route("[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public UsersController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }


        // the post i got this from had the AcceptVerbs but it doesn't look like it is necessary
        //[AcceptVerbs("GET", "POST")]
        [HttpGet("/VerifyKofCID/{KofCMemberID}")]
        //[Route("VerifyKofCID")]
        public async Task<IActionResult> VerifyKofCID(string KofCMemberID)
        
        {
            try
            {
                if (!IsKofCMemberIDValid(KofCMemberID))
                {
                    Log.Error("Invalid KofC Member ID");
                    return BadRequest("Invalid KofC Member ID");
                }
                //*******************************************************************************************
                // 6/22/2024 Tim Philomeno
                // simply return a TRUE if we find it and a FALSE if not
                // let the calling process deal with the validation logic and feedback
                //*******************************************************************************************
                var result = await _context.Database
                    .SqlQuery<KofCMemberIDUsers>($"EXECUTE uspSYS_ValidateKofCID {KofCMemberID} ")
                    .ToListAsync();

                if (result.Count() == 0)
                {
                    return Json(false);
                }
                else
                {
                    return Json(true);
                }
            }
            catch (Exception ex)
            {

                Log.Fatal("VerifyKofCIDAsync" + ex.Message + " " + ex.InnerException);
                return Json(false);
            }

        }
        private bool IsKofCMemberIDValid(string KofCMemberID)
        {
            //********************************************************************************************
            // 6/25/2024 Tim Philomeno
            // Rules for incoming parameter KofCMemberID
            // must be a string (this should be taken care of becasue of our parameter data type)
            // length must be between 5 and 7
            // must be 0-9
            //********************************************************************************************
            if (KofCMemberID.Length == 0)
            {
                return false;
            }
            if (KofCMemberID.Length < 5)
            {
                return false;
            }
            if (KofCMemberID.Length > 7)
            {
                return false;
            }

            string rxPat = (@"^[0-9]+$");

            if (!Regex.IsMatch(KofCMemberID, rxPat))
            {
                return false;
            }

            return true;
        }
    }
}

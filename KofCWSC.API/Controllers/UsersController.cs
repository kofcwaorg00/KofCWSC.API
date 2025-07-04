﻿using KofCWSC.API.Data;
using KofCWSC.API.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Serilog;
using System.Data;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KofCWSC.API.Controllers
{
    [Route("")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public UsersController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        [HttpGet("/VerifyLogin/{MemberLogin}")]
        public async Task<IActionResult> VerifyLogin(string MemberLogin)
        {
            //*******************************************************************************************
            // 6/22/2024 Tim Philomeno
            // simply return a TRUE if we find it and a FALSE if not
            // let the calling process deal with the validation logic and feedback
            // yes the logic is backwards from verify kofcid.  If we don't get any records
            // then the login is valid else it is suspended.
            //*******************************************************************************************
            var result = await _context.Database
                .SqlQuery<KofCMemberIDUsers>($"EXECUTE uspSYS_ValidateMemberLogin {MemberLogin} ")
                .ToListAsync();

            if (result.Count() == 0)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        // the post i got this from had the AcceptVerbs but it doesn't look like it is necessary
        //[AcceptVerbs("GET", "POST")]
        [HttpGet("/VerifyKofCID/{KofCMemberID}")]
        //[Route("VerifyKofCID")]
        public async Task<int> VerifyKofCID(int KofCMemberID)
        {
            try
            {
                var userIdParam = new SqlParameter("@KofCID", KofCMemberID);
                var verifyParam = new SqlParameter("@RetVal", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                _context.Database.ExecuteSqlRaw("EXEC dbo.uspSYS_ValidateKofCID @KofCID, @RetVal OUTPUT", userIdParam, verifyParam);

                return (int)verifyParam.Value;
                //if (!IsKofCMemberIDValid(KofCMemberID))
                //{
                //    var ex = new Exception($"Invalid KofC Member ID {KofCMemberID}");
                //    Log.Error(Utils.Helper.FormatLogEntry(this, ex));
                //    return BadRequest("Invalid KofC Member ID Format");
                //}
                ////*******************************************************************************************
                //// 6/22/2024 Tim Philomeno
                //// simply return a TRUE if we find it and a FALSE if not
                //// let the calling process deal with the validation logic and feedback
                ////*******************************************************************************************
                //var isMember = await _context.Database
                //    .SqlQuery<KofCMemberIDUsers>($"EXECUTE uspSYS_ValidateKofCID {KofCMemberID} ")
                //    .ToListAsync();

                //if (isMember.Count() > 0)
                //{
                //    // we have the KofC Member in our data, now check for suspensions
                //    var isSusp = await _context.Database
                //        .SqlQuery<KofCMemberIDUsers>($"EXECUTE uspSYS_IsMemberSuspended {KofCMemberID} ")
                //        .ToListAsync();
                //    if (isSusp.Count() > 0)
                //    {
                //        return Json("Member is Suspended! ");
                //    }
                //    else
                //    {
                //        return Json(true);
                //    }
                //}
                //else
                //{
                //    return Json(false);
                //}
            }
            catch (Exception ex)
            {

                Log.Fatal("VerifyKofCIDAsync" + ex.Message + " " + ex.InnerException);
                return -1;
            }

        }
        private bool IsKofCMemberIDValid(int KofCMemberID)
        {
            //********************************************************************************************
            // 6/25/2024 Tim Philomeno
            // Rules for incoming parameter KofCMemberID
            // must be a string (this should be taken care of becasue of our parameter data type)
            // length must be between 5 and 7
            // must be 0-9
            // must be within a range of 10000 to 6000000
            //********************************************************************************************
            if (KofCMemberID == 0)
            {
                return false;
            }
            if (KofCMemberID.ToString().Length < 5)
            {
                return false;
            }
            if (KofCMemberID.ToString().Length > 7)
            {
                return false;
            }
            if (!(KofCMemberID >= 10000 && KofCMemberID <= 6000000))
            {
                return false;
            }


            //string rxPat = (@"^[0-9]+$");

            //if (!Regex.IsMatch(KofCMemberID, rxPat))
            //{
            //    return false;
            //}

            return true;
        }
    }
}

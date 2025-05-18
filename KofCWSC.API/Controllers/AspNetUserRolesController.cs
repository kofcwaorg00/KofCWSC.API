using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using KofCWSC.API.Models;
using KofCWSC.API.Data;

namespace KofCWSC.API.Controllers
{
    public class AspNetUserRolesController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public AspNetUserRolesController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // POST: api/MemberOffices
        [HttpPost("UserRoles")]
        public async Task<ActionResult<AspNetUserRole>> CreateMemberOffice([FromBody] AspNetUserRole userRole)
        {
            _context.AspNetUserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            return Ok();
            //return CreatedAtAction(nameof(GetMemberOfficeDetails), new { id = memberOffice.Id }, memberOffice);
        }

        // DELETE: api/MemberOffices/5
        [HttpDelete("UserRole/{RoleId}/{UserId}")]
        public async Task<IActionResult> DeleteUserRole(string RoleId, string UserId)
        {
            var userRole = await _context.AspNetUserRoles.Where(u => u.RoleId == RoleId && u.UserId == UserId).ToListAsync();
            if (userRole == null)
            {
                return NotFound();
            }

            _context.AspNetUserRoles.Remove(userRole.FirstOrDefault());
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("UserRoles")]
        public void DeleteUserRoles(string userId)
        {
            ViewBag.UserId = userId;
        }
    }
}

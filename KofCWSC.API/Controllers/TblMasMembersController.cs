using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KofCWSC.API.Data;
using KofCWSC.API.Models;

namespace KofCWSC.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TblMasMembersController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public TblMasMembersController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: TblMasMembers
        [HttpGet(Name = "Members")]
        public async Task<IActionResult> Index()
        {
            //return Ok(await _context.TblMasMembers
            //    .Take(100)
            //    .ToListAsync());

            return Json(await _context.TblMasMembers.Take(100).ToListAsync());
        }

        //////////// GET: TblMasMembers/Details/5
        //////////[ApiExplorerSettings(IgnoreApi = true)]
        //////////public async Task<IActionResult> Details(int? id)
        //////////{
        //////////    if (id == null)
        //////////    {
        //////////        return NotFound();
        //////////    }

        //////////    var tblMasMember = await _context.TblMasMember
        //////////        .FirstOrDefaultAsync(m => m.MemberId == id);
        //////////    if (tblMasMember == null)
        //////////    {
        //////////        return NotFound();
        //////////    }

        //////////    return View(tblMasMember);
        //////////}

        //////////// GET: TblMasMembers/Create
        //////////[ApiExplorerSettings(IgnoreApi = true)]
        //////////public IActionResult Create()
        //////////{
        //////////    return View();
        //////////}

        //////////// POST: TblMasMembers/Create
        //////////// To protect from overposting attacks, enable the specific properties you want to bind to.
        //////////// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //////////[HttpPost]
        //////////[ValidateAntiForgeryToken]
        //////////public async Task<IActionResult> Create([Bind("MemberId,KofCid,Prefix,PrefixUpdated,PrefixUpdatedBy,FirstName,FirstNameUpdated,FirstNameUpdatedBy,NickName,NickNameUpdated,NickNameUpdatedBy,Mi,Miupdated,MiupdatedBy,LastName,LastNameUpdated,LastNameUpdatedBy,Suffix,SuffixUpdated,SuffixUpdatedBy,AddInfo1,AddInfo1Updated,AddInfo1UpdatedBy,Address,AddressUpdated,AddressUpdatedBy,City,CityUpdated,CityUpdatedBy,State,StateUpdated,StateUpdatedBy,PostalCode,PostalCodeUpdated,PostalCodeUpdatedBy,Phone,PhoneUpdated,PhoneUpdatedBy,WifesName,WifesNameUpdated,WifesNameUpdatedBy,AddInfo2,AddInfo2Updated,AddInfo2UpdatedBy,FaxNumber,FaxNumberUpdated,FaxNumberUpdatedBy,Council,CouncilUpdated,CouncilUpdatedBy,Assembly,AssemblyUpdated,AssemblyUpdatedBy,Circle,CircleUpdated,CircleUpdatedBy,Email,EmailUpdated,EmailUpdatedBy,Deceased,DeceasedUpdated,DeceasedUpdatedBy,CellPhone,CellPhoneUpdated,CellPhoneUpdatedBy,LastUpdated,SeatedDelegateDay1,SeatedDelegateDay2,SeatedDelegateDay3,PaidMpd,Bulletin,BulletinUpdated,BulletinUpdatedBy,UserId,Data,DataChanged,LastLoggedIn,CanEditAdmUi,DoNotEmail,HidePersonalInfo,WhyDoNotEmail")] TblMasMember tblMasMember)
        //////////{
        //////////    if (ModelState.IsValid)
        //////////    {
        //////////        _context.Add(tblMasMember);
        //////////        await _context.SaveChangesAsync();
        //////////        return RedirectToAction(nameof(Index));
        //////////    }
        //////////    return View(tblMasMember);
        //////////}

        //////////// GET: TblMasMembers/Edit/5
        //////////[ApiExplorerSettings(IgnoreApi = true)]
        //////////public async Task<IActionResult> Edit(int? id)
        //////////{
        //////////    if (id == null)
        //////////    {
        //////////        return NotFound();
        //////////    }

        //////////    var tblMasMember = await _context.TblMasMember.FindAsync(id);
        //////////    if (tblMasMember == null)
        //////////    {
        //////////        return NotFound();
        //////////    }
        //////////    return View(tblMasMember);
        //////////}

        //////////// POST: TblMasMembers/Edit/5
        //////////// To protect from overposting attacks, enable the specific properties you want to bind to.
        //////////// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //////////[HttpPost]
        //////////[ValidateAntiForgeryToken]
        //////////public async Task<IActionResult> Edit(int id, [Bind("MemberId,KofCid,Prefix,PrefixUpdated,PrefixUpdatedBy,FirstName,FirstNameUpdated,FirstNameUpdatedBy,NickName,NickNameUpdated,NickNameUpdatedBy,Mi,Miupdated,MiupdatedBy,LastName,LastNameUpdated,LastNameUpdatedBy,Suffix,SuffixUpdated,SuffixUpdatedBy,AddInfo1,AddInfo1Updated,AddInfo1UpdatedBy,Address,AddressUpdated,AddressUpdatedBy,City,CityUpdated,CityUpdatedBy,State,StateUpdated,StateUpdatedBy,PostalCode,PostalCodeUpdated,PostalCodeUpdatedBy,Phone,PhoneUpdated,PhoneUpdatedBy,WifesName,WifesNameUpdated,WifesNameUpdatedBy,AddInfo2,AddInfo2Updated,AddInfo2UpdatedBy,FaxNumber,FaxNumberUpdated,FaxNumberUpdatedBy,Council,CouncilUpdated,CouncilUpdatedBy,Assembly,AssemblyUpdated,AssemblyUpdatedBy,Circle,CircleUpdated,CircleUpdatedBy,Email,EmailUpdated,EmailUpdatedBy,Deceased,DeceasedUpdated,DeceasedUpdatedBy,CellPhone,CellPhoneUpdated,CellPhoneUpdatedBy,LastUpdated,SeatedDelegateDay1,SeatedDelegateDay2,SeatedDelegateDay3,PaidMpd,Bulletin,BulletinUpdated,BulletinUpdatedBy,UserId,Data,DataChanged,LastLoggedIn,CanEditAdmUi,DoNotEmail,HidePersonalInfo,WhyDoNotEmail")] TblMasMember tblMasMember)
        //////////{
        //////////    if (id != tblMasMember.MemberId)
        //////////    {
        //////////        return NotFound();
        //////////    }

        //////////    if (ModelState.IsValid)
        //////////    {
        //////////        try
        //////////        {
        //////////            _context.Update(tblMasMember);
        //////////            await _context.SaveChangesAsync();
        //////////        }
        //////////        catch (DbUpdateConcurrencyException)
        //////////        {
        //////////            if (!TblMasMemberExists(tblMasMember.MemberId))
        //////////            {
        //////////                return NotFound();
        //////////            }
        //////////            else
        //////////            {
        //////////                throw;
        //////////            }
        //////////        }
        //////////        return RedirectToAction(nameof(Index));
        //////////    }
        //////////    return View(tblMasMember);
        //////////}

        //////////// GET: TblMasMembers/Delete/5
        //////////[ApiExplorerSettings(IgnoreApi = true)]
        //////////public async Task<IActionResult> Delete(int? id)
        //////////{
        //////////    if (id == null)
        //////////    {
        //////////        return NotFound();
        //////////    }

        //////////    var tblMasMember = await _context.TblMasMember
        //////////        .FirstOrDefaultAsync(m => m.MemberId == id);
        //////////    if (tblMasMember == null)
        //////////    {
        //////////        return NotFound();
        //////////    }

        //////////    return View(tblMasMember);
        //////////}

        //////////// POST: TblMasMembers/Delete/5
        //////////[HttpPost, ActionName("Delete")]
        //////////[ValidateAntiForgeryToken]
        //////////public async Task<IActionResult> DeleteConfirmed(int id)
        //////////{
        //////////    var tblMasMember = await _context.TblMasMember.FindAsync(id);
        //////////    if (tblMasMember != null)
        //////////    {
        //////////        _context.TblMasMember.Remove(tblMasMember);
        //////////    }

        //////////    await _context.SaveChangesAsync();
        //////////    return RedirectToAction(nameof(Index));
        //////////}

        //////////private bool TblMasMemberExists(int id)
        //////////{
        //////////    return _context.TblMasMember.Any(e => e.MemberId == id);
        //////////}
    }
}

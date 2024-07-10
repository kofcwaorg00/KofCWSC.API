﻿using System;
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
    public class TblValAssysController : Controller
    {
        private readonly KofCWSCAPIDBContext _context;

        public TblValAssysController(KofCWSCAPIDBContext context)
        {
            _context = context;
        }

        // GET: TblValAssys
        public async Task<IActionResult> Index()
        {
            return View(await _context.TblValAssy
                .OrderBy(x => x.ANumber)
                .ToListAsync());
        }

        // GET: TblValAssys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblValAssy = await _context.TblValAssy
                .FirstOrDefaultAsync(m => m.ANumber == id);
            if (tblValAssy == null)
            {
                return NotFound();
            }

            return View(tblValAssy);
        }

        // GET: TblValAssys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TblValAssys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ANumber,ALocation,AName,AddInfo1,AddInfo2,AddInfo3,WebSiteUrl,MasterLoc")] TblValAssy tblValAssy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblValAssy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblValAssy);
        }

        // GET: TblValAssys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblValAssy = await _context.TblValAssy.FindAsync(id);
            if (tblValAssy == null)
            {
                return NotFound();
            }
            return View(tblValAssy);
        }

        // POST: TblValAssys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ANumber,ALocation,AName,AddInfo1,AddInfo2,AddInfo3,WebSiteUrl,MasterLoc")] TblValAssy tblValAssy)
        {
            if (id != tblValAssy.ANumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblValAssy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblValAssyExists(tblValAssy.ANumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tblValAssy);
        }

        // GET: TblValAssys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblValAssy = await _context.TblValAssy
                .FirstOrDefaultAsync(m => m.ANumber == id);
            if (tblValAssy == null)
            {
                return NotFound();
            }

            return View(tblValAssy);
        }

        // POST: TblValAssys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblValAssy = await _context.TblValAssy.FindAsync(id);
            if (tblValAssy != null)
            {
                _context.TblValAssy.Remove(tblValAssy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblValAssyExists(int id)
        {
            return _context.TblValAssy.Any(e => e.ANumber == id);
        }
    }
}

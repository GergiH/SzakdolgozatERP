using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Authorization;

namespace ERPSzakdolgozat.Controllers
{
	[Authorize(Policy = "Admin")]
	public class UnitsController : Controller
    {
        private readonly ERPDBContext _context;

        public UnitsController(ERPDBContext context)
        {
            _context = context;
        }

		// GET: Units
		public async Task<IActionResult> Index()
        {
            var units = _context.Units.AsNoTracking();
            return View(await units.ToListAsync());
        }

		// GET: Units/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Units.FirstOrDefaultAsync(m => m.Id == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

		// GET: Units/Create
		public IActionResult Create()
        {
			Unit unit = new Unit
			{
				Active = true
			};

            return View(unit);
        }

		// POST: Units/Create
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Unit unit)
        {
            if (ModelState.IsValid)
            {
				unit.CreatedDate = DateTime.Now;
				unit.ModifiedDate = DateTime.Now;
				unit.Id = _context.Units.Max(t => t.Id) + 1;

                _context.Add(unit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(unit);
        }

		// GET: Units/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Units.FindAsync(id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // POST: Units/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Unit unit)
        {
            if (id != unit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnitExists(unit.Id))
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

            return View(unit);
        }

		// GET: Units/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Units.FirstOrDefaultAsync(m => m.Id == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

		// POST: Units/Delete/5
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unit = await _context.Units.FindAsync(id);
            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UnitExists(int id)
        {
            return _context.Units.Any(e => e.Id == id);
        }
    }
}

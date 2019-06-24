using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERPSzakdolgozat.Models;
using ERPSzakdolgozat.Helpers;

namespace ERPSzakdolgozat.Controllers
{
    public class WorkDaysController : MyController
    {
        public WorkDaysController(ERPDBContext context) : base(context)
        {
            _context = context;
        }

        // GET: WorkDays
        public async Task<IActionResult> Index()
        {
            return View(await _context.WorkDays.ToListAsync());
        }

        // GET: WorkDays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workDay = await _context.WorkDays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workDay == null)
            {
                return NotFound();
            }

            return View(workDay);
        }

        // GET: WorkDays/Create
        public IActionResult Create()
        {
			WorkDay workday = new WorkDay();

            return View(workday);
        }

        // POST: WorkDays/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkDay workDay)
        {
            if (ModelState.IsValid)
            {
				workDay.Id = _context.WorkDays.Max(w => w.Id) + 1;
				workDay.CreatedDate = DateTime.Now;
				workDay.ModifiedDate = DateTime.Now;

				_context.Add(workDay);
                await _context.SaveChangesAsync();

				TempData["Toast"] = Toasts.Created;
                return RedirectToAction(nameof(Index));
            }
            return View(workDay);
        }

        // GET: WorkDays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workDay = await _context.WorkDays.FindAsync(id);
            if (workDay == null)
            {
                return NotFound();
            }
            return View(workDay);
        }

        // POST: WorkDays/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkDay workDay)
        {
            if (id != workDay.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workDay);
                    await _context.SaveChangesAsync();

					TempData["Toast"] = Toasts.Saved;
				}
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkDayExists(workDay.Id))
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
            return View(workDay);
        }

        // GET: WorkDays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workDay = await _context.WorkDays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workDay == null)
            {
                return NotFound();
            }

            return View(workDay);
        }

        // POST: WorkDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workDay = await _context.WorkDays.FindAsync(id);
            _context.WorkDays.Remove(workDay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkDayExists(int id)
        {
            return _context.WorkDays.Any(e => e.Id == id);
        }
    }
}

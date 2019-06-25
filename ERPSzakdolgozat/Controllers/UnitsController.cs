using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Controllers
{
	[Authorize(Policy = "Admin")]
	public class UnitsController : MyController
	{
		public UnitsController(ERPDBContext context) : base(context)
		{
			_context = context;
		}

		// GET: Units
		public async Task<IActionResult> Index()
		{
			var units = _context.Units.AsNoTracking();
			return View(await units.ToListAsync());
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

				TempData["Toast"] = Toasts.Created;
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

					TempData["Toast"] = Toasts.Saved;
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

			unit.Teams = await _context.Teams.Where(t => t.UnitId == unit.Id).ToListAsync();
			if (unit.Teams == null)
				ViewData["TeamCount"] = 0;
			else
				ViewData["TeamCount"] = 1;

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

			TempData["Toast"] = Toasts.Deleted;
			return RedirectToAction(nameof(Index));
		}

		private bool UnitExists(int id)
		{
			return _context.Units.Any(e => e.Id == id);
		}
	}
}
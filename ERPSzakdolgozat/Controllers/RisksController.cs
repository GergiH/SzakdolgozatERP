using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Controllers
{
	public class RisksController : MyController
	{
		public RisksController(ERPDBContext context) : base(context)
		{
			_context = context;
		}

		// GET: Risks
		public async Task<IActionResult> Index()
		{
			return View(await _context.Risks.OrderBy(r => r.RiskName).ToListAsync());
		}

		// GET: Risks/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var risk = await _context.Risks
				.FirstOrDefaultAsync(m => m.Id == id);
			if (risk == null)
			{
				return NotFound();
			}

			return View(risk);
		}

		// GET: Risks/Create
		public IActionResult Create()
		{
			Risk risk = new Risk();
			return View(risk);
		}

		// POST: Risks/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Risk risk)
		{
			if (ModelState.IsValid)
			{
				risk.CreatedDate = DateTime.Now;
				risk.ModifiedDate = DateTime.Now;
				risk.Id = _context.Risks.Max(c => c.Id) + 1;

				_context.Add(risk);
				await _context.SaveChangesAsync();

				TempData["Toast"] = Toasts.Created;
				return RedirectToAction(nameof(Index));
			}
			return View(risk);
		}

		// GET: Risks/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var risk = await _context.Risks.FindAsync(id);
			if (risk == null)
			{
				return NotFound();
			}
			return View(risk);
		}

		// POST: Risks/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Risk risk)
		{
			if (id != risk.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(risk);
					await _context.SaveChangesAsync();

					TempData["Toast"] = Toasts.Saved;
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RiskExists(risk.Id))
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
			return View(risk);
		}

		// GET: Risks/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var risk = await _context.Risks
				.FirstOrDefaultAsync(m => m.Id == id);
			if (risk == null)
			{
				return NotFound();
			}

			return View(risk);
		}

		// POST: Risks/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var risk = await _context.Risks.FindAsync(id);
			_context.Risks.Remove(risk);
			await _context.SaveChangesAsync();

			TempData["Toast"] = Toasts.Deleted;
			return RedirectToAction(nameof(Index));
		}

		private bool RiskExists(int id)
		{
			return _context.Risks.Any(e => e.Id == id);
		}
	}
}
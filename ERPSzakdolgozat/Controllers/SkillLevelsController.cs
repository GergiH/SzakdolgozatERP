using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Controllers
{
	public class SkillLevelsController : MyController
	{
		public SkillLevelsController(ERPDBContext context) : base(context)
		{
			_context = context;
		}

		// GET: SkillLevels
		public async Task<IActionResult> Index()
		{
			return View(await _context.SkillLevels.ToListAsync());
		}

		// GET: SkillLevels/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var skillLevel = await _context.SkillLevels
				.FirstOrDefaultAsync(m => m.Id == id);
			if (skillLevel == null)
			{
				return NotFound();
			}

			return View(skillLevel);
		}

		// GET: SkillLevels/Create
		public IActionResult Create()
		{
			SkillLevel sl = new SkillLevel();
			return View(sl);
		}

		// POST: SkillLevels/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(SkillLevel skillLevel)
		{
			if (ModelState.IsValid)
			{
				skillLevel.CreatedDate = DateTime.Now;
				skillLevel.ModifiedDate = DateTime.Now;
				skillLevel.Id = _context.SkillLevels.Max(t => t.Id) + 1;

				_context.Add(skillLevel);
				await _context.SaveChangesAsync();

				TempData["Toast"] = Toasts.Created;
				return RedirectToAction(nameof(Index));
			}
			return View(skillLevel);
		}

		// GET: SkillLevels/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var skillLevel = await _context.SkillLevels.FindAsync(id);
			if (skillLevel == null)
			{
				return NotFound();
			}
			return View(skillLevel);
		}

		// POST: SkillLevels/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, SkillLevel skillLevel)
		{
			if (id != skillLevel.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(skillLevel);
					await _context.SaveChangesAsync();

					TempData["Toast"] = Toasts.Saved;
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SkillLevelExists(skillLevel.Id))
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
			return View(skillLevel);
		}

		// GET: SkillLevels/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var skillLevel = await _context.SkillLevels
				.FirstOrDefaultAsync(m => m.Id == id);
			if (skillLevel == null)
			{
				return NotFound();
			}

			return View(skillLevel);
		}

		// POST: SkillLevels/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var skillLevel = await _context.SkillLevels.FindAsync(id);
			_context.SkillLevels.Remove(skillLevel);
			await _context.SaveChangesAsync();

			TempData["Toast"] = Toasts.Deleted;
			return RedirectToAction(nameof(Index));
		}

		private bool SkillLevelExists(int id)
		{
			return _context.SkillLevels.Any(e => e.Id == id);
		}
	}
}
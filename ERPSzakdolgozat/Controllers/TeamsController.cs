using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Controllers
{
	public class TeamsController : MyController
	{
		public TeamsController(ERPDBContext context) : base(context)
		{
			_context = context;
		}

		// GET: Teams
		[Authorize(Policy = "Admin")]
		public async Task<IActionResult> Index()
		{
			var teamList = _context.Teams.AsNoTracking().Include(t => t.Unit);
			return View(await teamList.ToListAsync());
		}

		// GET: Teams/Create
		[Authorize(Policy = "Admin")]
		public IActionResult Create()
		{
			ViewData["UnitId"] = new SelectList(_context.Units, "Id", "UnitName");

			Team team = new Team
			{
				Active = true
			};

			return View(team);
		}

		// POST: Teams/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Admin")]
		public async Task<IActionResult> Create(Team team)
		{
			if (ModelState.IsValid)
			{
				team.CreatedDate = DateTime.Now;
				team.ModifiedDate = DateTime.Now;
				team.Id = _context.Teams.Max(t => t.Id) + 1;

				_context.Add(team);
				await _context.SaveChangesAsync();

				TempData["Toast"] = Toasts.Created;
				return RedirectToAction(nameof(Index));
			}

			ViewData["UnitId"] = new SelectList(_context.Units, "Id", "UnitName", team.UnitId);
			return View(team);
		}

		// GET: Teams/Edit/5
		[Authorize(Policy = "Admin")]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Team team = await _context.Teams.FindAsync(id);
			if (team == null)
			{
				return NotFound();
			}

			ViewData["UnitId"] = new SelectList(_context.Units, "Id", "UnitName", team.UnitId);
			return View(team);
		}

		// POST: Teams/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Admin")]
		public async Task<IActionResult> Edit(int id, Team team)
		{
			if (id != team.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(team);
					await _context.SaveChangesAsync();

					TempData["Toast"] = Toasts.Saved;
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!TeamExists(team.Id))
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
			ViewData["UnitId"] = new SelectList(_context.Units, "Id", "UnitName", team.UnitId);
			return View(team);
		}

		// GET: Teams/Delete/5
		[Authorize(Policy = "Admin")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Team team = await _context.Teams
				.Include(t => t.Unit)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (team == null)
			{
				return NotFound();
			}

			team.Employees = await _context.Employees.Where(e => e.TeamId == team.Id).ToListAsync();
			if (team.Employees == null)
				ViewData["EmployeeCount"] = 0;
			else
				ViewData["EmployeeCount"] = 1;

			return View(team);
		}

		// POST: Teams/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var team = await _context.Teams.FindAsync(id);
			_context.Teams.Remove(team);
			await _context.SaveChangesAsync();

			TempData["Toast"] = Toasts.Deleted;
			return RedirectToAction(nameof(Index));
		}

		private bool TeamExists(int id)
		{
			return _context.Teams.Any(e => e.Id == id);
		}
	}
}
using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Controllers
{
	public class ProjectsController : MyController
	{
		private readonly ERPDBContext _context;

		public ProjectsController(ERPDBContext context)
		{
			_context = context;
		}

		// GET: Projects
		public async Task<IActionResult> Index()
		{
			var eRPDBContext = _context.Projects
				.Include(p => p.Client)
				.OrderBy(p => p.ProjectName);
			return View(await eRPDBContext.ToListAsync());
		}

		// GET: Projects/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var project = await _context.Projects
				.Include(p => p.Client)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (project == null)
			{
				return NotFound();
			}

			return View(project);
		}

		// GET: Projects/Create
		public IActionResult Create()
		{
			Project proj = new Project();
			ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "ClientId");
			return View(proj);
		}

		// POST: Projects/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Project project)
		{
			if (ModelState.IsValid)
			{
				project.CreatedDate = DateTime.Now;
				project.ModifiedDate = DateTime.Now;
				project.Id = _context.Projects.Max(p => p.Id) + 1;

				_context.Add(project);
				await _context.SaveChangesAsync();

				TempData["Toast"] = Toasts.Created;
				return RedirectToAction(nameof(Index));
			}
			ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "ClientId", project.ClientId);
			return View(project);
		}

		// GET: Projects/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var project = await _context.Projects.FindAsync(id);
			if (project == null)
			{
				return NotFound();
			}
			ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "ClientId", project.ClientId);
			return View(project);
		}

		// POST: Projects/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Project project)
		{
			if (id != project.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(project);
					TempData["Toast"] = Toasts.Saved;
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProjectExists(project.Id))
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
			ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "ClientId", project.ClientId);
			return View(project);
		}

		// GET: Projects/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var project = await _context.Projects
				.Include(p => p.Client)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (project == null)
			{
				return NotFound();
			}

			return View(project);
		}

		// POST: Projects/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var project = await _context.Projects.FindAsync(id);
			_context.Projects.Remove(project);
			await _context.SaveChangesAsync();

			TempData["Toast"] = Toasts.Deleted;
			return RedirectToAction(nameof(Index));
		}

		private bool ProjectExists(int id)
		{
			return _context.Projects.Any(e => e.Id == id);
		}
	}
}
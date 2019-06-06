using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using ERPSzakdolgozat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Controllers
{
	public class ProjectsController : MyController
	{
		public ProjectsController(ERPDBContext context) : base(context)
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

			project.Resources = await _context.ProjectResources
				.Where(r => r.ProjectId == project.Id)
				.OrderByDescending(r => r.CreatedDate)
				.ToListAsync();
			project.Logs = await _context.ProjectLogs
				.Include("AppUser")
				.Where(l => l.ProjectId == project.Id)
				.OrderByDescending(l => l.CreatedDate)
				.ToListAsync();
			project.Risks = await _context.ProjectRisks
				.Where(r => r.ProjectId == project.Id)
				.ToListAsync();

			await FillDropdownLists(project);
			ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "ClientName", project.ClientId);

			Project_Edit peVM = new Project_Edit
			{
				Project = project,
				NewProjectResource = new ProjectResource(),
				EmployeeSelectList = new SelectList(_context.Employees, "Id", "EmployeeName"),
				SubSelectList = new SelectList(_context.Subcontractors, "Id", "SubcontractorName")
			};

			CalculateFinancials(peVM);
			await _context.SaveChangesAsync();

			return View(peVM);
		}

		// POST: Projects/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Project_Edit peVM)
		{
			if (id != peVM.Project.Id)
			{
				return NotFound();
			}

			//var errors = ModelState.Select(x => x.Value.Errors)
			//	.Where(y => y.Count > 0)
			//	.Select(a => a[0].ErrorMessage)
			//	.ToList();

			if (ModelState.IsValid)
			{
				try
				{
					CalculateFinancials(peVM);
					_context.Update(peVM.Project);
					await _context.SaveChangesAsync();

					TempData["Toast"] = Toasts.Saved;
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProjectExists(peVM.Project.Id))
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

			await FillDropdownLists(peVM.Project);
			ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "ClientName", peVM.Project.ClientId);

			return View(peVM);
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

		private async Task FillDropdownLists(Project proj)
		{
			ViewData["Currencies"] = new SelectList(await _context.Currencies
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.CurrencyName
				})
				.OrderBy(c => c.Text)
				.ToListAsync(), "Value", "Text");
			ViewData["Types"] = new SelectList(
				new List<SelectListItem>
				{
					new SelectListItem { Value = "Fixed price", Text = "Fixed price" },
					new SelectListItem { Value = "Time and material", Text = "Time and material" }
				}, "Value", "Text");
			ViewData["ProjectStatuses"] = new SelectList(
				new List<SelectListItem>
				{
					new SelectListItem { Value = "Not started", Text = "Not started" },
					new SelectListItem { Value = "Executing", Text = "Executing" },
					new SelectListItem { Value = "Paused", Text = "Paused" },
					new SelectListItem { Value = "Failed", Text = "Failed" },
					new SelectListItem { Value = "Finished", Text = "Finished" }
				}, "Value", "Text");
			ViewData["ContractStatuses"] = new SelectList(
				new List<SelectListItem>
				{
					new SelectListItem { Value = "Not started", Text = "Not started" },
					new SelectListItem { Value = "In progress", Text = "In progress" },
					new SelectListItem { Value = "Signed", Text = "Signed" },
					new SelectListItem { Value = "Rejected", Text = "Rejected" }
				}, "Value", "Text");
			ViewData["ResourceTypes"] = new List<SelectListItem>
				{
					new SelectListItem { Value = "Employee", Text = "Employee" },
					new SelectListItem { Value = "Subcontractor", Text = "Subcontractor" },
					new SelectListItem { Value = "Other", Text = "Other" },
				};

			List<SelectListItem> projMans = await _context.AppUsers
				.Where(u => u.Roles.Any(r => r.RoleID == 3) == true)
				.Select(u => new SelectListItem
				{
					Value = u.ADName,
					Text = u.DisplayName
				})
				.ToListAsync();
			// if the user is not a Project Manager anymore, add it to the list
			if (!projMans.Any(p => p.Value == proj.ProjectManager))
			{
				projMans.Add(new SelectListItem
				{
					Value = proj.ProjectManager,
					Text = await _context.AppUsers
						.Where(u => u.ADName == proj.ProjectManager)
						.Select(u => u.DisplayName)
						.FirstOrDefaultAsync()
				});
			}

			ViewData["ProjectManagers"] = new SelectList(projMans.OrderBy(p => p.Text), "Value", "Text");

			ViewData["LogUsers"] = new SelectList(proj.Logs
				.GroupBy(l => l.UserId)
				.Select(l => l.FirstOrDefault())
				.Select(l => new SelectListItem
				{
					Value = l.UserId.ToString(),
					Text = l.AppUser.DisplayName
				})
				.OrderBy(l => l.Text)
				.ToList(), "Value", "Text");
		}

		public JsonResult CalculateFinancials(Project_Edit peVM)
		{
			double hDone = 0,
				hRem = 0,
				oDone = 0,
				oRem = 0,
				cs = 0,
				cr = 0,
				rs = 0,
				rr = 0,
				resRevGained = 0,
				resRev = 0,
				riskRev = 0;

			// recalculate all resources
			foreach (ProjectResource res in peVM.Project.Resources)
			{
				hDone += res.HoursDone;
				hRem += res.HoursRemaining;
				oDone += res.OvertimeDone;
				oRem += res.OvertimeRemaining;
				cs += res.HoursDone * res.Cost;
				cr += res.HoursRemaining * res.Cost;
				resRevGained += Globals.CalculateRevenue(res.HoursDone, res.OvertimeDone, res.Cost);
				resRev += Globals.CalculateRevenue((res.HoursDone + res.HoursRemaining), (res.OvertimeDone + res.OvertimeRemaining), res.Cost);
			}

			// TODO add risk calculations

			peVM.Project.HoursDone = hDone;
			peVM.Project.HoursRemaining = hRem;
			peVM.Project.HoursAll = hDone + hRem;
			peVM.Project.OvertimeDone = oDone;
			peVM.Project.OvertimeRemaining = oRem;
			peVM.Project.OvertimeAll = oDone + oRem;
			peVM.Project.ResourcesCostSpent = cs;
			peVM.Project.ResourcesCostRemaining = cr;
			peVM.Project.ResourcesCost = cs + cr;
			peVM.Project.RiskCostSpent = rs;
			peVM.Project.RiskCostRemaining = rr;
			peVM.Project.RiskCost = rs + rr;
			peVM.Project.ResourcesRevenueGained = resRevGained;
			peVM.Project.ResourcesRevenue = resRev;
			peVM.Project.RiskRevenue = riskRev;

			return Json(peVM);
		}

		public async Task<IActionResult> AddResource(
			int id,
			string resName,
			int? resEmp,
			int? resSub,
			string task,
			double hDone,
			double hRem,
			double oDone,
			double oRem,
			double cost
		)
		{
			if (!string.IsNullOrEmpty(resName))
			{
				double hAll = hDone + hRem;
				double oAll = oDone + oRem;

				ProjectResource newPR = new ProjectResource
				{
					Cost = cost,
					CreatedDate = DateTime.Now,
					HoursAll = hAll,
					HoursDone = hDone,
					HoursRemaining = hRem,
					ModifiedDate = DateTime.Now,
					OvertimeAll = oAll,
					OvertimeDone = oDone,
					OvertimeRemaining = oRem,
					ProjectId = id,
					ResourceName = resName,
					ResourceEmployee = resEmp,
					ResourceSubcontractor = resSub,
					Revenue = Globals.CalculateRevenue(hDone, oDone, cost),
					ResourceTask = task,
					Id = _context.ProjectResources.Max(r => r.Id) + 1
				};

				_context.Add(newPR);
				await _context.SaveChangesAsync();
			}
			
			// TODO controller authorize
			return RedirectToAction("Edit", id);
		}
	}
}
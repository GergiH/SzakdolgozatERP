using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using ERPSzakdolgozat.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
		public async Task<IActionResult> Index(string search, DateTime? started, bool active = true)
		{
			IQueryable<Project> projects = _context.Projects
				.Include(p => p.Client)
				.Include(p => p.Risks)
				.AsNoTracking();

			if (!string.IsNullOrEmpty(search))
			{
				projects = projects
					.Where(p => p.ProjectName.ToLower().Contains(search.ToLower())
						|| p.Type.ToLower().Contains(search.ToLower())
						|| p.ProjectManager.ToLower().Contains(search.ToLower()));
			}
			if (started != null)
			{
				projects = projects.Where(p => p.StartDate >= started);
			}
			if (active)
			{
				projects = projects.Where(p => p.EndDate == null || p.EndDate >= DateTime.Now);
			}

			ViewData["search"] = search;
			ViewData["started"] = started?.ToString("yyyy-MM-dd");
			ViewData["active"] = active;

			List<Project> prList = await projects.OrderBy(p => p.ProjectName).ToListAsync();

			// fill Risks
			foreach (Project pr in prList)
			{
				foreach (var rk in pr.Risks)
				{
					rk.Risk = await _context.Risks.FindAsync(rk.RiskId);
				}
			}

			return View(prList);
		}

		// GET: Projects/Details/5
		[Authorize(Policy = "ProjectManager")]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var project = await _context.Projects.FirstOrDefaultAsync(m => m.Id == id);
			if (project == null)
			{
				return NotFound();
			}

			// fill Project's entity lists
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

			foreach (var pr in project.Risks)
			{
				pr.Risk = await _context.Risks.FindAsync(pr.RiskId);
			}

			await FillDropdownLists(project);
			ViewData["ClientId"] = new SelectList(_context.Clients.OrderBy(c => c.ClientName), "Id", "ClientName", project.ClientId);

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

		// GET: Projects/Create
		[Authorize(Policy = "ProjectManager")]
		public async Task<IActionResult> Create()
		{
			Project proj = new Project
			{
				StartDate = DateTime.Now,
				EstimatedEndDate = DateTime.Now.AddDays(30)
			};

			await FillDropdownLists(proj);
			ViewData["ClientId"] = new SelectList(_context.Clients.OrderBy(c => c.ClientName), "Id", "ClientName");
			return View(proj);
		}

		// POST: Projects/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "ProjectManager")]
		public async Task<IActionResult> Create(Project project)
		{
			if (ModelState.IsValid)
			{
				project.CreatedDate = DateTime.Now;
				project.ModifiedDate = DateTime.Now;
				project.Id = _context.Projects.Max(p => p.Id) + 1;

				_context.Add(project);
				await _context.SaveChangesAsync();

				List<Risk> allRisks = await _context.Risks.ToListAsync();
				foreach (Risk risk in allRisks)
				{
					ProjectRisk newPR = new ProjectRisk
					{
						CreatedDate = DateTime.Now,
						ModifiedDate = DateTime.Now,
						IsSelected = false,
						ProjectId = project.Id,
						RiskId = risk.Id,
						Id = _context.ProjectRisks.Max(r => r.Id) + 1
					};

					await _context.AddAsync(newPR);
					await _context.SaveChangesAsync();
				}

				TempData["Toast"] = Toasts.Created;
				return RedirectToAction(nameof(Index));
			}
			ViewData["ClientId"] = new SelectList(_context.Clients.OrderBy(c => c.ClientName), "Id", "ClientName", project.ClientId);
			return View(project);
		}

		// GET: Projects/Edit/5
		[Authorize(Policy = "ProjectManager")]
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

			// fill Project's entity lists
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
			
			foreach (var pr in project.Risks)
			{
				pr.Risk = await _context.Risks.FindAsync(pr.RiskId);
			}

			await FillDropdownLists(project);
			ViewData["ClientId"] = new SelectList(_context.Clients.OrderBy(c => c.ClientName), "Id", "ClientName", project.ClientId);

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
		[Authorize(Policy = "ProjectManager")]
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
			ViewData["ClientId"] = new SelectList(_context.Clients.OrderBy(c => c.ClientName), "Id", "ClientName", peVM.Project.ClientId);

			return View(peVM);
		}

		// GET: Projects/Delete/5
		[Authorize(Policy = "ProjectManager")]
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
		[Authorize(Policy = "ProjectManager")]
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

			if (proj.Logs != null)
			{
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

			// risk stuff
			if (proj.Risks != null)
			{
				ViewData["SelectedWeight"] = proj.Risks.Where(r => r.IsSelected == true).Sum(r => r.Risk.RiskWeight);
				ViewData["TotalWeight"] = proj.Risks.Sum(r => r.Risk.RiskWeight);
			}
		}

		public JsonResult CalculateFinancials(Project_Edit peVM)
		{
			double hDone = 0,
				hRem = 0,
				oDone = 0,
				oRem = 0,
				cs = 0,
				cr = 0,
				resRevGained = 0,
				resRev = 0;

			// recalculate all resources
			if (peVM.Project.Resources != null)
			{
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

					res.Revenue = Globals.CalculateRevenue(res.HoursDone, res.OvertimeDone, res.Cost);
				}
			}

			peVM.Project.HoursDone = hDone;
			peVM.Project.HoursRemaining = hRem;
			peVM.Project.HoursAll = hDone + hRem;
			peVM.Project.OvertimeDone = oDone;
			peVM.Project.OvertimeRemaining = oRem;
			peVM.Project.OvertimeAll = oDone + oRem;
			peVM.Project.ResourcesCostSpent = cs;
			peVM.Project.ResourcesCostRemaining = cr;
			peVM.Project.ResourcesCost = cs + cr;
			peVM.Project.RiskCost = peVM.Project.RiskCostSpent + peVM.Project.RiskCostRemaining;
			peVM.Project.ResourcesRevenueGained = resRevGained;
			peVM.Project.ResourcesRevenue = resRev;
			peVM.Project.TotalCostSpent = cs + peVM.Project.RiskCostSpent;
			peVM.Project.TotalCostRemaining = cr + peVM.Project.RiskCostRemaining;
			peVM.Project.TotalCost = cs + cr + peVM.Project.RiskCostSpent + peVM.Project.RiskCostRemaining;
			peVM.Project.TotalRevenue = resRev + peVM.Project.RiskRevenue;

			return Json(peVM);
		}

		[Authorize(Policy = "ProjectManager")]
		public JsonResult AddResource(
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
				_context.SaveChanges();
			}

			return Json(null);
		}

		[Authorize(Policy = "ProjectManager")]
		public async Task<string> DeleteResource(int id)
		{
			if (id != 0)
			{
				ProjectResource pr = await _context.ProjectResources.FindAsync(id);
				_context.Remove(pr);
				await _context.SaveChangesAsync();
			}

			return null;
		}
	}
}
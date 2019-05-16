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
	public class EmployeesController : MyController
	{
		private readonly ERPDBContext _context;
		private Dictionary<int, string> _employeeNames;
		private Dictionary<int, string> _teamNames;

		public EmployeesController(ERPDBContext context)
		{
			_context = context;
			_employeeNames = _context.Employees
				.AsNoTracking()
				.ToDictionary(e => e.Id, e => e.EmployeeName);
			_teamNames = _context.Teams
				.AsNoTracking()
				.ToDictionary(t => t.Id, t => t.TeamName);
		}

		// GET: Employees
		public async Task<IActionResult> Index(string search, bool active = true)
		{
			IQueryable<Employee_Index> employeeVMList = _context.Employees
				.AsNoTracking()
				.Select(e => new Employee_Index
				{
					Active = e.Active,
					Address = e.HomeCountry + ", " + e.HomeZIP + " " + e.HomeCity + " - " + e.HomeStreet,
					CompanyIdentifier = e.CompanyIdentifier,
					CreatedDate = e.CreatedDate,
					Email = e.Email,
					Id = e.Id,
					LeaderName = _employeeNames.FirstOrDefault(x => x.Key == e.LeaderId).Value,
					Mobile = e.Mobile,
					ModifiedDate = e.ModifiedDate,
					EmployeeName = e.EmployeeName,
					TeamName = _teamNames.FirstOrDefault(t => t.Key == e.TeamId).Value
				});

			if (!string.IsNullOrEmpty(search))
			{
				employeeVMList = employeeVMList
					.Where(e => e.EmployeeName.Contains(search, StringComparison.CurrentCultureIgnoreCase)
						|| e.CompanyIdentifier.Contains(search, StringComparison.CurrentCultureIgnoreCase));
			}

			if (active == true)
			{
				employeeVMList = employeeVMList.Where(e => e.Active == true);
			}
			else
			{
				employeeVMList = employeeVMList.Where(e => e.Active == false);
			}

			employeeVMList = employeeVMList.OrderBy(e => e.EmployeeName);

			// Setting ViewBag values for the view to keep the filters after reloading the page
			ViewData["search"] = search;
			ViewData["active"] = active;

			return View(await employeeVMList.ToListAsync());
		}

		// GET: Employees/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Employee employee = await _context.Employees
				.AsNoTracking()
				.Include(e => e.EmployeeFinancials)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (employee == null)
			{
				return NotFound();
			}

			employee.EmployeeFinancials = employee.EmployeeFinancials.OrderByDescending(e => e.ActiveFrom).ToList();

			FillDropdownLists();

			ViewData["LeaderName"] = _employeeNames
				.Where(e => e.Key == employee.LeaderId)
				.Select(e => e.Value)
				.FirstOrDefault();
			ViewData["TeamName"] = _teamNames
				.Where(e => e.Key == employee.TeamId)
				.Select(e => e.Value)
				.FirstOrDefault();

			return View(employee);
		}

		// GET: Employees/Create
		public IActionResult Create()
		{
			Employee employee = new Employee
			{
				Active = true,
				JoinedOn = DateTime.Now
			};
			FillDropdownLists();

			return View(employee);
		}

		// POST: Employees/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Employee employee)
		{
			if (ModelState.IsValid)
			{
				employee.Id = _context.Employees.Max(e => e.Id) + 1;

				_context.Add(employee);
				await _context.SaveChangesAsync();

				EmployeeFinancial employeeFinancial = new EmployeeFinancial
				{
					ActiveFrom = DateTime.Now,
					Bonus = 0,
					Cafeteria = 0,
					CreatedDate = DateTime.Now,
					CurrencyId = 1,
					EmployeeId = employee.Id,
					GrossSalary = 0,
					Id = _context.EmployeeFinancials.Max(e => e.Id) + 1,
					ModifiedDate = DateTime.Now,
					WorkHours = 0
				};
				_context.Add(employeeFinancial);
				await _context.SaveChangesAsync();

				TempData["Toast"] = Toasts.Created;
				return RedirectToAction(nameof(Index));
			}
			return View(employee);
		}

		// GET: Employees/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Employee employee = await _context.Employees
				.Include(e => e.EmployeeFinancials)
				.FirstOrDefaultAsync(e => e.Id == id);
			if (employee == null)
			{
				return NotFound();
			}

			employee.EmployeeFinancials = employee.EmployeeFinancials.OrderByDescending(e => e.ActiveFrom).ToList();

			FillDropdownLists();

			return View(employee);
		}

		// POST: Employees/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Employee employee)
		{
			if (id != employee.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(employee);
					await _context.SaveChangesAsync();

					TempData["Toast"] = Toasts.Saved;
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!EmployeeExists(employee.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}

				// TODO if leave date is on, active should be false

				return RedirectToAction(nameof(Index));
			}
			return View(employee);
		}

		// GET: Employees/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees
				.FirstOrDefaultAsync(m => m.Id == id);
			if (employee == null)
			{
				return NotFound();
			}

			return View(employee);
		}

		// POST: Employees/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var employee = await _context.Employees.FindAsync(id);
			_context.Employees.Remove(employee);
			await _context.SaveChangesAsync();

			TempData["Toast"] = Toasts.Deleted;
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Statistics()
		{
			return View();
		}

		public int AddFinancial(int currencyId, int workHours, double grossSalary, double cafeteria, double bonus, int employeeId)
		{
			try
			{
				EmployeeFinancial newFinancial = new EmployeeFinancial
				{
					ActiveFrom = DateTime.Now,
					Bonus = bonus,
					Cafeteria = cafeteria,
					CreatedDate = DateTime.Now,
					CurrencyId = currencyId,
					EmployeeId = employeeId,
					GrossSalary = grossSalary,
					Id = _context.EmployeeFinancials.Max(e => e.Id) + 1,
					ModifiedDate = DateTime.Now,
					WorkHours = workHours
				};

				_context.EmployeeFinancials.Add(newFinancial);
				_context.SaveChanges();
			}
			catch
			{
				return 1; // return a value to make JS success/error message in 'employee_edit.js'
			}

			return 0;
		}

		private bool EmployeeExists(int id)
		{
			return _context.Employees.Any(e => e.Id == id);
		}

		private void FillDropdownLists()
		{
			ViewData["Leaders"] = _context.Employees
				.AsNoTracking()
				.Where(e => e.IsLeader)
				.Select(e => new SelectListItem
				{
					Value = e.Id.ToString(),
					Text = e.EmployeeName
				})
				.ToList();

			ViewData["Teams"] = _teamNames
				.Select(t => new SelectListItem
				{
					Value = t.Key.ToString(),
					Text = t.Value
				})
				.ToList();

			ViewData["Currencies"] = _context.Currencies
				.AsNoTracking()
				.Select(c => new SelectListItem
				{
					Value = c.Id.ToString(),
					Text = c.CurrencyName
				})
				.ToList();

			ViewData["Roles"] = _context.Roles
				.AsNoTracking()
				.Where(r => r.IsSelectable == true)
				.Select(r => new SelectListItem
				{
					Value = r.Id.ToString(),
					Text = r.RoleName
				})
				.ToList();

			ViewData["SkillLevels"] = _context.SkillLevels
				.AsNoTracking()
				.Where(s => s.IsSelectable == true)
				.Select(s => new SelectListItem
				{
					Value = s.Id.ToString(),
					Text = s.SkillLevelName
				})
				.ToList();
		}
	}
}
using ERPSzakdolgozat.Models;
using ERPSzakdolgozat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Controllers
{
	public class EmployeesController : Controller
	{
		private readonly ERPDBContext _context;
		private Dictionary<int, string> employeeNames;
		private Dictionary<int, string> teamNames;

		public EmployeesController(ERPDBContext context)
		{
			_context = context;
			employeeNames = _context.Employees
				.AsNoTracking()
				.ToDictionary(e => e.Id, e => e.Name);
			teamNames = _context.Teams
				.AsNoTracking()
				.ToDictionary(t => t.Id, t => t.Name);
		}

		// GET: Employees
		public IActionResult Index()
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
					LeaderName = employeeNames.FirstOrDefault(x => x.Key == e.LeaderId).Value,
					Mobile = e.Mobile,
					ModifiedDate = e.ModifiedDate,
					Name = e.Name,
					TeamName = teamNames.FirstOrDefault(t => t.Key == e.TeamId).Value
				});

			return View(employeeVMList);
		}

		// GET: Employees/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees
				.Include(e => e.EmployeeFinancials)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (employee == null)
			{
				return NotFound();
			}

			FillDropdownLists();
			ViewBag.IsDetails = "true";
			ViewBag.LeaderName = _context.Employees
				.Where(e => e.Id == employee.LeaderId)
				.Select(e => e.Name)
				.FirstOrDefault();
			ViewBag.TeamName = _context.Teams
				.Where(e => e.Id == employee.TeamId)
				.Select(e => e.Name)
				.FirstOrDefault();

			return View("Edit", employee);
		}

		// GET: Employees/Create
		public IActionResult Create()
		{
			Employee employee = new Employee();
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
				// TODO create a financial record as well

				_context.Add(employee);
				await _context.SaveChangesAsync();
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

			var employee = await _context.Employees
				.Include(e => e.EmployeeFinancials)
				.FirstOrDefaultAsync(e => e.Id == id);
			if (employee == null)
			{
				return NotFound();
			}

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
			return RedirectToAction(nameof(Index));
		}

		private bool EmployeeExists(int id)
		{
			return _context.Employees.Any(e => e.Id == id);
		}

		private void FillDropdownLists()
		{
			ViewBag.Leaders = _context.Employees
				.Where(e => e.IsLeader)
				.Select(e => new SelectListItem
				{
					Value = e.Id.ToString(),
					Text = e.Name
				});

			ViewBag.Teams = _context.Teams.Select(t => new SelectListItem
			{
				Value = t.Id.ToString(),
				Text = t.Name
			});

			ViewBag.Currencies = _context.Currencies.Select(c => new SelectListItem
			{
				Value = c.Id.ToString(),
				Text = c.Name
			});

			ViewBag.Roles = _context.Roles
				.Where(r => r.IsSelectable == true)
				.Select(r => new SelectListItem
				{
					Value = r.Id.ToString(),
					Text = r.Name
				});

			ViewBag.SkillLevels = _context.SkillLevels
				.Where(s => s.IsSelectable == true)
				.Select(s => new SelectListItem
				{
					Value = s.Id.ToString(),
					Text = s.Name
				});
		}
	}
}
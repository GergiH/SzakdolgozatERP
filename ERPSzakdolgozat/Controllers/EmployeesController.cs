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

		public EmployeesController(ERPDBContext context)
		{
			_context = context;
		}

		// GET: Employees
		public IActionResult Index()
		{
			IEnumerable<Employee_Index> employeeVMList = _context.Employees.Select(e => new Employee_Index
			{
				Active = e.Active,
				Address = e.HomeCountry + ", " + e.HomeZIP + " " + e.HomeCity + " - " + e.HomeStreet,
				CompanyIdentifier = e.CompanyIdentifier,
				CreatedDate = e.CreatedDate,
				Email = e.Email,
				Id = e.Id,
				LeaderName = _context.Employees.Where(x => x.Id == e.LeaderId).FirstOrDefault().Name,
				Mobile = e.Mobile,
				ModifiedDate = e.ModifiedDate,
				Name = e.Name,
				TeamName = _context.Teams.Where(t => t.Id == e.TeamId).FirstOrDefault().Name
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
				.FirstOrDefaultAsync(m => m.Id == id);
			if (employee == null)
			{
				return NotFound();
			}

			return View(employee);
		}

		// GET: Employees/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Employees/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Employee employee)
		{
			if (ModelState.IsValid)
			{
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

			var employee = await _context.Employees.FindAsync(id);
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
			ViewBag.Leaders = _context.Employees.Where(e => e.IsLeader).Select(e => new SelectListItem
			{
				Value = e.Id.ToString(),
				Text = e.Name
			});
		}
	}
}
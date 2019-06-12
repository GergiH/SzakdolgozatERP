using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace ERPSzakdolgozat.Controllers
{
	public class HomeController : MyController
	{
		public HomeController(ERPDBContext context) : base(context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			double[] salaries = new double[12];

			double[] hoursDone = new double[12];
			double[] hoursTotal = new double[12];

			double[] cost = new double[12];
			double[] revenue = new double[12];
			double[] revenueGained = new double[12];

			for (int i = 0; i < 12; i++)
			{
				salaries[i] = _context.EmployeeFinancials.Where(e => e.ActiveFrom.Month == i + 1).DefaultIfEmpty().Average(e => e.GrossSalary);

				hoursDone[i] = _context.Projects.Where(p => p.ModifiedDate.Month == i + 1).Sum(p => p.HoursDone);
				hoursTotal[i] = _context.Projects.Where(p => p.ModifiedDate.Month == i + 1).Sum(p => p.HoursAll);

				cost[i] = _context.Projects.Where(p => p.ModifiedDate.Month == i + 1).Sum(p => p.TotalCost);
				revenue[i] = _context.Projects.Where(p => p.ModifiedDate.Month == i + 1).Sum(p => p.TotalRevenue);
				revenueGained[i] = _context.Projects.Where(p => p.ModifiedDate.Month == i + 1).Sum(p => p.ResourcesRevenueGained);
			}

			// Employees
			ViewData["Salaries"] = salaries;

			// Projects
			// 1st chart
			ViewData["HoursDone"] = hoursDone;
			ViewData["HoursTotal"] = hoursTotal;

			// 2nd chart
			ViewData["HoursRemaining"] = _context.Projects.Sum(p => p.HoursRemaining);
			ViewData["HoursAll"] = _context.Projects.Sum(p => p.HoursAll);

			// 3rd chart
			ViewData["Cost"] = cost;
			ViewData["Revenue"] = revenue;
			ViewData["RevenueGained"] = revenueGained;

			return View();
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["AdminEmails"] = _context.AppUsers
				.AsNoTracking()
				.Where(u => u.Roles.Any(r => r.RoleID == 1) == true)
				.Select(u => u.Email)
				.ToList();

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

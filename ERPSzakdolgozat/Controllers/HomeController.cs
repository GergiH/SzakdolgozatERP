using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
			// Employees
			double[] salaries = new double[12];

			// Projects
			double[] hoursDone = new double[12];
			double[] hoursTotal = new double[12];
			double[] cost = new double[12];
			double[] revenue = new double[12];
			double[] revenueGained = new double[12];

			// Forecasts
			double[] avaHours = new double[12];
			double[] totalHours = new double[12];

			bool isAdmin = User.HasClaim(ClaimTypes.Role, "Admin");
			bool isHR = User.HasClaim(ClaimTypes.Role, "HR");
			bool isPM = User.HasClaim(ClaimTypes.Role, "ProjectManager");
			bool isTL = User.HasClaim(ClaimTypes.Role, "TeamLeader");

			List<Employee> employees = new List<Employee>();
			if (isAdmin || isHR)
			{
				employees = _context.Employees
				   .Include(e => e.EmployeeFinancials)
				   .AsNoTracking()
				   .ToList();
			}

			for (int i = 0; i < 12; i++)
			{
				if (isAdmin || isHR)
				{
					double grossesForMonth = 0;
					double countOfGrosses = 0;
					double actualGross = 0;
					foreach (Employee emp in employees)
					{
						actualGross += emp.EmployeeFinancials
							.Where(f => f.ActiveFrom.Month == i + 1)
							.Select(f => f.GrossSalary)
							.FirstOrDefault(); // TODO if multiple records are present for the same month, a random one is selected (should get the latest of the month)
						if (actualGross != 0)
							countOfGrosses++;
						grossesForMonth += actualGross;
					}

					salaries[i] = countOfGrosses == 0 ? 0 : grossesForMonth / countOfGrosses;
				}

				if (isAdmin || isPM)
				{
					hoursDone[i] = _context.Projects.Where(p => p.ModifiedDate.Month == i + 1).Sum(p => p.HoursDone);
					hoursTotal[i] = _context.Projects.Where(p => p.ModifiedDate.Month == i + 1).Sum(p => p.HoursAll);

					cost[i] = _context.Projects.Where(p => p.ModifiedDate.Month == i + 1).Sum(p => p.TotalCost);
					revenue[i] = _context.Projects.Where(p => p.ModifiedDate.Month == i + 1).Sum(p => p.TotalRevenue);
					revenueGained[i] = _context.Projects.Where(p => p.ModifiedDate.Month == i + 1).Sum(p => p.ResourcesRevenueGained);
				}

				if (isAdmin || isTL)
				{
					avaHours[i] = _context.ForecastWeeks.Where(f => f.WeekStart.Month == i + 1).Sum(f => f.HoursAvailable);
					totalHours[i] = _context.ForecastWeeks.Where(f => f.WeekStart.Month == i + 1).Sum(f => f.HoursAll);
				}
			}

			// Employees
			if (isAdmin || isHR)
			{
				ViewData["Salaries"] = salaries;
			}

			// Projects
			if (isAdmin || isPM)
			{
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
			}

			// Forecasts
			if (isAdmin || isTL)
			{
				ViewData["AvailableHours"] = avaHours;
				ViewData["TotalHours"] = totalHours;
			}

			// texts on the top
			int currentHour = DateTime.Now.TimeOfDay.Hours;
			if (currentHour >= 6 && currentHour <= 11)
			{
				ViewData["TimeOfDay"] = "morning";
			}
			else if (currentHour > 11 && currentHour <= 17)
			{
				ViewData["TimeOfDay"] = "afternoon";
			}
			else if (currentHour > 17 && currentHour <= 21)
			{
				ViewData["TimeOfDay"] = "evening";
			}
			else
			{
				ViewData["TimeOfDay"] = "night;";
			}

			ViewData["UserName"] = _context.AppUsers
				.Where(a => a.ADName == User.Identity.Name)
				.Select(a => a.DisplayName)
				.FirstOrDefault();

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

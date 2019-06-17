using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERPSzakdolgozat.Models;
using System.Globalization;
using ERPSzakdolgozat.Helpers;

namespace ERPSzakdolgozat.Controllers
{
	public class ForecastsController : MyController
	{
		private readonly HashSet<int> _employeeIds;

		public ForecastsController(ERPDBContext context) : base(context)
		{
			_context = context;
			_employeeIds = context.Employees
				.Where(e => e.Active)
				.Select(e => e.Id)
				.ToHashSet();
		}

		// GET: Forecasts
		public async Task<IActionResult> Index(int? team, int? employee, int? weekNumber)
		{
			IQueryable<ForecastWeek> forecastWeeks = _context.ForecastWeeks.Include(f => f.Employee);

			foreach (ForecastWeek fw in forecastWeeks)
			{
				fw.Forecasts = await _context.Forecast
					.Where(f => f.ForecastWeekId == fw.Id)
					.Include(f => f.Employee)
					.Include(f => f.ForecastWeek)
					.Include(f => f.Project)
					.ToListAsync();
			}

			int currentWeekNumber = Globals.GetCurrentWeekNumber();
			weekNumber = weekNumber ?? currentWeekNumber;

			if (employee != null)
				forecastWeeks = forecastWeeks.Where(f => f.Employee.Id == employee);
			if (team != null)
				forecastWeeks = forecastWeeks.Where(f => f.Employee.TeamId == team);
			if (weekNumber != null) // probably redundant but good to be sure
				forecastWeeks = forecastWeeks.Where(f => f.WeekNumber == weekNumber);

			ViewData["Employees"] = new SelectList(_context.Employees, "Id", "EmployeeName", employee);
			ViewData["Teams"] = new SelectList(_context.Teams, "Id", "TeamName", team);
			ViewData["Weeks"] = new SelectList(
				_context.ForecastWeeks
					.GroupBy(w => w.WeekNumber)
					.Select(w => new SelectListItem
					{
						Value = w.Key.ToString(),
						Text = w.First().WeekNumber.ToString() + " - " + w.First().WeekStart.ToShortDateString()
					})
					.OrderBy(w => w.Text)
					.ToList(),
				"Value", "Text", weekNumber);

			int plusWeeks = 6;
			int.TryParse(_context.AppSettings
				.FirstOrDefault(s => s.SettingName == "Forecast - Generate Weeks")
				.SettingValue, out plusWeeks);
			int addedMaxWeek = Globals.GetCurrentWeekNumber() + plusWeeks;
			int lastWeekNo = _context.ForecastWeeks
					.Where(f => f.WeekStart.Year == DateTime.Today.Year)
					.Max(f => f.WeekNumber);
			lastWeekNo = lastWeekNo < Globals.GetCurrentWeekNumber()
				? Globals.GetCurrentWeekNumber()
				: lastWeekNo;

			ViewData["GenerateWeeksNumber"] = lastWeekNo == 0
				? plusWeeks
				: addedMaxWeek - lastWeekNo;

			return View(await forecastWeeks.ToListAsync());
		}

		// GET: Forecasts/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var forecast = await _context.Forecast
				.Include(f => f.Employee)
				.Include(f => f.ForecastWeek)
				.Include(f => f.Project)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (forecast == null)
			{
				return NotFound();
			}

			return View(forecast);
		}

		// GET: Forecasts/Create
		public IActionResult Create()
		{
			ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "CompanyIdentifier");
			ViewData["ForecastWeekId"] = new SelectList(_context.Set<ForecastWeek>(), "Id", "Id");
			ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "Contract");
			return View();
		}

		// POST: Forecasts/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Forecast forecast)
		{
			if (ModelState.IsValid)
			{
				_context.Add(forecast);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "CompanyIdentifier", forecast.EmployeeId);
			ViewData["ForecastWeekId"] = new SelectList(_context.Set<ForecastWeek>(), "Id", "Id", forecast.ForecastWeekId);
			ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "Contract", forecast.ProjectID);
			return View(forecast);
		}

		// GET: Forecasts/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var forecastWeek = await _context.ForecastWeeks.FindAsync(id);
			if (forecastWeek == null)
			{
				return NotFound();
			}

			var forecasts = await _context.Forecast.Where(f => f.ForecastWeekId == id).ToListAsync();
			FillViewData(forecastWeek);

			return View(forecasts);
		}

		// POST: Forecasts/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(List<Forecast> forecasts)
		{
			if (ModelState.IsValid)
			{
				try
				{
					double pro = 0;
					double vac = 0;
					double sic = 0;
					double tra = 0;
					double ben = 0;

					foreach (var item in forecasts)
					{
						switch (item.LeaveName)
						{
							case "Vacation":
								vac += item.Hours;
								break;
							case "Sickness":
								sic += item.Hours;
								break;
							case "Training":
								tra += item.Hours;
								break;
							case "Bench":
								ben += item.Hours;
								break;
							default:
								pro += item.Hours;
								break;
						}
						_context.Update(item);
					}

					ForecastWeek fw = await _context.ForecastWeeks.FindAsync(forecasts[0].ForecastWeekId);
					fw.ProjectHours = pro;
					fw.VacationHours = vac;
					fw.SicknessHours = sic;
					fw.TrainingHours = tra;
					fw.BenchHours = ben;
					fw.HoursAll = pro + vac + sic + tra + ben;

					_context.Update(fw);
					await _context.SaveChangesAsync();

					TempData["Toast"] = Toasts.Saved;

					return RedirectToAction("Edit");
				}
				catch (DbUpdateConcurrencyException) { throw; }
			}

			return View(forecasts);
		}

		public void FillViewData(ForecastWeek forecastWeek)
		{
			ViewData["ForecastTypes"] = new SelectList(new List<SelectListItem>
			{
				new SelectListItem { Value = "Project", Text = "Project" },
				new SelectListItem { Value = "Vacation", Text = "Vacation" },
				new SelectListItem { Value = "Sickness", Text = "Sickness" },
				new SelectListItem { Value = "Training", Text = "Training" },
				new SelectListItem { Value = "Bench", Text = "Bench" }
			}, "Value", "Text");
			ViewData["Projects"] = new SelectList(_context.Projects.OrderBy(p => p.ProjectName), "Id", "ProjectName");

			ViewData["NewForecast"] = new Forecast
			{
				CreatedDate = DateTime.Now,
				EmployeeId = forecastWeek.EmployeeId,
				ForecastType = "Project",
				ForecastWeekId = forecastWeek.Id,
				ModifiedDate = DateTime.Now,
				Hours = 0
			};
			ViewData["Total"] = forecastWeek.HoursAll;
		}

		// TODO deleteforecast, savenewforecast, generate weeks, details page

		// GET: Forecasts/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var forecast = await _context.Forecast
				.Include(f => f.Employee)
				.Include(f => f.ForecastWeek)
				.Include(f => f.Project)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (forecast == null)
			{
				return NotFound();
			}

			return View(forecast);
		}

		// POST: Forecasts/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var forecast = await _context.Forecast.FindAsync(id);
			_context.Forecast.Remove(forecast);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ForecastExists(int id)
		{
			return _context.Forecast.Any(e => e.Id == id);
		}

		public async Task<IActionResult> GenerateWeeks()
		{
			int plusWeeks = 6;
			int.TryParse(_context.AppSettings
				.FirstOrDefault(s => s.SettingName == "Forecast - Generate Weeks")
				.SettingValue, out plusWeeks);
			int addedMaxWeek = Globals.GetCurrentWeekNumber() + plusWeeks;
			int maxWeekId = _context.ForecastWeeks.Max(f => f.Id);

			// if there are forecast weeks in the year
			if (_context.ForecastWeeks.Any(f => f.WeekStart.Year == DateTime.Today.Year))
			{
				int lastWeekNo = _context.ForecastWeeks
					.Where(f => f.WeekStart.Year == DateTime.Today.Year)
					.Max(f => f.WeekNumber);
				DateTime lastWeekStart = _context.ForecastWeeks
					.Where(f => f.WeekStart.Year == DateTime.Today.Year && f.WeekNumber == lastWeekNo)
					.FirstOrDefault()
					.WeekStart;

				// it there are forecast weeks missing
				if (lastWeekNo < addedMaxWeek)
				{
					while (lastWeekNo < addedMaxWeek)
					{
						if (lastWeekNo > 52)
							lastWeekNo = 1;
						lastWeekStart = lastWeekStart.AddDays(7);
						lastWeekNo++;

						// TODO szünnapok tábla

						foreach (int id in _employeeIds)
						{
							maxWeekId++;
							ForecastWeek newFW = new ForecastWeek
							{
								BenchHours = 0,
								CreatedDate = DateTime.Now,
								EmployeeId = id,
								HoursAll = 0,
								HoursAvailable = 40,
								ModifiedDate = DateTime.Now,
								ProjectHours = 0,
								SicknessHours = 0,
								TrainingHours = 0,
								VacationHours = 0,
								WeekNumber = lastWeekNo,
								WeekStart = lastWeekStart,
								Id = maxWeekId
							};

							await _context.ForecastWeeks.AddAsync(newFW);
						}
						await _context.SaveChangesAsync();
					}
				}
			}
			else
			{
				// this not covers if there were records last year, but missing some weeks (should not happen ever anyway)
				int lastWeekNo = 1;
				DateTime lastWeekStart = new DateTime(DateTime.Today.Year, 1, 1)
					.AddDays(-(int)new DateTime(DateTime.Today.Year, 1, 1).DayOfWeek + (int)DayOfWeek.Monday);

				while (lastWeekNo < addedMaxWeek)
				{
					if (lastWeekNo > 52)
						lastWeekNo = 1;
					lastWeekStart = lastWeekStart.AddDays(7);
					lastWeekNo++;

					foreach (int id in _employeeIds)
					{
						maxWeekId++;
						ForecastWeek newFW = new ForecastWeek
						{
							BenchHours = 0,
							CreatedDate = DateTime.Now,
							EmployeeId = id,
							HoursAll = 0,
							HoursAvailable = 40,
							ModifiedDate = DateTime.Now,
							ProjectHours = 0,
							SicknessHours = 0,
							TrainingHours = 0,
							VacationHours = 0,
							WeekNumber = lastWeekNo,
							WeekStart = lastWeekStart,
							Id = maxWeekId
						};

						await _context.ForecastWeeks.AddAsync(newFW);
					}
					await _context.SaveChangesAsync();
				}
			}

			return RedirectToAction("Index");
		}
	}
}

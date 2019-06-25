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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ERPSzakdolgozat.Controllers
{
	// TODO Excel export if possible within the time limit
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

			int? currentWeekNumber = TempData["WeekNumber"] == null ? null : (int?)TempData["WeekNumber"];
			weekNumber = weekNumber ?? currentWeekNumber;

			if (employee != null)
				forecastWeeks = forecastWeeks.Where(f => f.Employee.Id == employee);
			if (team != null)
				forecastWeeks = forecastWeeks.Where(f => f.Employee.TeamId == team);
			if (weekNumber != null)
			{
				forecastWeeks = forecastWeeks.Where(f => f.WeekNumber == weekNumber);
				TempData["WeekNumber"] = weekNumber;
			}

			if (team == null)
				ViewData["Employees"] = new SelectList(_context.Employees.OrderBy(e => e.EmployeeName), "Id", "EmployeeName", employee);
			else
				ViewData["Employees"] = new SelectList(_context.Employees.Where(e => e.TeamId == team).OrderBy(e => e.EmployeeName), "Id", "EmployeeName", employee);
			ViewData["Teams"] = new SelectList(_context.Teams.OrderBy(t => t.TeamName), "Id", "TeamName", team);
			ViewData["Weeks"] = new SelectList(
				_context.ForecastWeeks
					.GroupBy(w => w.WeekStart)
					.Select(w => new SelectListItem
					{
						Value = w.First().WeekNumber.ToString(),
						Text = w.Key.ToString("yyyy-MM-dd") + " - " + w.First().WeekNumber.ToString()
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

			return View(await forecastWeeks.OrderBy(f => f.Employee.EmployeeName).ToListAsync());
		}

		// GET: Forecasts/Details/5
		public async Task<IActionResult> Details(int? id)
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

		// GET: Forecasts/Edit/5
		[Authorize(Policy = "TeamLeader")]
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
		[Authorize(Policy = "TeamLeader")]
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

		private bool ForecastExists(int id)
		{
			return _context.Forecast.Any(e => e.Id == id);
		}

		[Authorize(Policy = "TeamLeader")]
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

						double hA = 40;
						List<WorkDay> workdays = await _context.WorkDays
							.Where(w => w.WorkDayDate >= lastWeekStart && w.WorkDayDate <= lastWeekStart.AddDays(7))
							.ToListAsync();
						
						if (workdays.Count > 0)
						{
							foreach (var item in workdays)
							{
								if (item.IsHoliday)
									hA -= 8;
								else
									hA += 8;
							}
						}

						foreach (int id in _employeeIds)
						{
							maxWeekId++;
							ForecastWeek newFW = new ForecastWeek
							{
								BenchHours = 0,
								CreatedDate = DateTime.Now,
								EmployeeId = id,
								HoursAll = 0,
								HoursAvailable = hA,
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

					double hA = 40;
					List<WorkDay> workdays = await _context.WorkDays
						.Where(w => w.WorkDayDate >= lastWeekStart && w.WorkDayDate <= lastWeekStart.AddDays(7))
						.ToListAsync();

					if (workdays.Count > 0)
					{
						foreach (var item in workdays)
						{
							if (item.IsHoliday)
								hA -= 8;
							else
								hA += 8;
						}
					}

					foreach (int id in _employeeIds)
					{
						maxWeekId++;
						ForecastWeek newFW = new ForecastWeek
						{
							BenchHours = 0,
							CreatedDate = DateTime.Now,
							EmployeeId = id,
							HoursAll = 0,
							HoursAvailable = hA,
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

		[Authorize(Policy = "TeamLeader")]
		public async Task<string> AddForecast(
			string fcType,
			int? fcProjectID,
			int? fcForecastWeekID,
			int? fcEmployeeID,
			double fcHours,
			string fcComment)
		{
			// if anything important is missing, do nothing
			if (string.IsNullOrEmpty(fcType) || fcProjectID == null || fcForecastWeekID == null || fcEmployeeID == null)
				return null;

			Forecast newFC = new Forecast
			{
				Comment = fcComment,
				CreatedDate = DateTime.Now,
				EmployeeId = fcEmployeeID.Value,
				ForecastType = fcType,
				ForecastWeekId = fcForecastWeekID.Value,
				Hours = fcHours,
				Id = _context.Forecast.Max(f => f.Id) + 1,
				LeaveName = fcType == "Project" ? null : fcType,
				ModifiedDate = DateTime.Now,
				ProjectID = fcProjectID
			};

			ForecastWeek fw = await _context.ForecastWeeks.FindAsync(fcForecastWeekID);
			switch (fcType)
			{
				case "Vacation":
					fw.VacationHours += fcHours;
					break;
				case "Sickness":
					fw.SicknessHours += fcHours;
					break;
				case "Training":
					fw.TrainingHours += fcHours;
					break;
				case "Bench":
					fw.BenchHours += fcHours;
					break;
				default:
					fw.ProjectHours += fcHours;
					break;
			}
			fw.HoursAll = fw.VacationHours + fw.SicknessHours + fw.TrainingHours + fw.BenchHours + fw.ProjectHours;
			_context.Update(fw);

			await _context.Forecast.AddAsync(newFC);
			await _context.SaveChangesAsync();

			return null;
		}

		[Authorize(Policy = "TeamLeader")]
		public async Task<string> DeleteForecast(int id)
		{
			if (id != 0)
			{
				Forecast fc = await _context.Forecast.FindAsync(id);
				ForecastWeek fw = await _context.ForecastWeeks.FindAsync(fc.ForecastWeekId);
				switch (fc.ForecastType)
				{
					case "Vacation":
						fw.VacationHours -= fc.Hours;
						break;
					case "Sickness":
						fw.SicknessHours -= fc.Hours;
						break;
					case "Training":
						fw.TrainingHours -= fc.Hours;
						break;
					case "Bench":
						fw.BenchHours -= fc.Hours;
						break;
					default:
						fw.ProjectHours -= fc.Hours;
						break;
				}
				fw.HoursAll = fw.VacationHours + fw.SicknessHours + fw.TrainingHours + fw.BenchHours + fw.ProjectHours;
				_context.Update(fw);
				_context.Remove(fc);

				await _context.SaveChangesAsync();
			}

			return null;
		}

		[Authorize(Policy = "Admin")]
		public async Task<IActionResult> FixThisWeek(int fwId)
		{
			ForecastWeek fw = await _context.ForecastWeeks.FindAsync(fwId);
			List<ForecastWeek> fWeeks = await _context.ForecastWeeks
				.Where(f => f.WeekStart == fw.WeekStart)
				.ToListAsync();

			int maxWeekId = _context.ForecastWeeks.Max(f => f.Id);
			foreach (int id in _employeeIds)
			{
				if (!fWeeks.Any(f => f.EmployeeId == id))
				{
					double hA = 40;
					List<WorkDay> workdays = await _context.WorkDays
						.Where(w => w.WorkDayDate >= fw.WeekStart && w.WorkDayDate <= fw.WeekStart.AddDays(7))
						.ToListAsync();

					if (workdays.Count > 0)
					{
						foreach (var item in workdays)
						{
							if (item.IsHoliday)
								hA -= 8;
							else
								hA += 8;
						}
					}

					maxWeekId++;
					ForecastWeek newFW = new ForecastWeek
					{
						BenchHours = 0,
						CreatedDate = DateTime.Now,
						EmployeeId = id,
						HoursAll = 0,
						HoursAvailable = hA,
						ModifiedDate = DateTime.Now,
						ProjectHours = 0,
						SicknessHours = 0,
						TrainingHours = 0,
						VacationHours = 0,
						WeekNumber = fw.WeekNumber,
						WeekStart = fw.WeekStart,
						Id = maxWeekId
					};

					await _context.ForecastWeeks.AddAsync(newFW);
				}
			}
			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}
	}
}

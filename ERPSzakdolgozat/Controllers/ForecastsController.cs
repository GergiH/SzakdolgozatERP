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
        public ForecastsController(ERPDBContext context) : base(context)
		{
            _context = context;
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

			// get the current week number
			CultureInfo culture = CultureInfo.CurrentCulture;
			int currentWeekNumber = culture.Calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
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
					.Select(w => new SelectListItem
					{
						Value = w.WeekNumber.ToString(),
						Text = w.WeekNumber.ToString() + " - " + w.WeekStart.ToShortDateString()
					})
					.OrderBy(w => w.Text)
					.ToList(),
				"Value", "Text", weekNumber);

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
					foreach (var item in forecasts)
					{
						_context.Update(item);
					}
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
    }
}

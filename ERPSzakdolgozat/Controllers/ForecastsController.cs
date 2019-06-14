using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERPSzakdolgozat.Models;
using System.Globalization;

namespace ERPSzakdolgozat.Controllers
{
    public class ForecastsController : Controller
    {
        private readonly ERPDBContext _context;

        public ForecastsController(ERPDBContext context)
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

			ViewData["Employees"] = new SelectList(
				_context.Employees
					.Select(e => new SelectListItem
					{
						Value = e.Id.ToString(),
						Text = e.EmployeeName
					})
					.OrderBy(e => e.Text)
					.ToList(),
				"Value", "Text", employee);
			ViewData["Teams"] = new SelectList(
				_context.Teams
					.Select(t => new SelectListItem
					{
						Value = t.Id.ToString(),
						Text = t.TeamName
					})
					.OrderBy(t => t.Text)
					.ToList(),
				"Value", "Text", team);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,ForecastWeekId,ProjectID,ForecastType,Hours,Id,CreatedDate,ModifiedDate")] Forecast forecast)
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

            var forecast = await _context.Forecast.FindAsync(id);
            if (forecast == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "CompanyIdentifier", forecast.EmployeeId);
            ViewData["ForecastWeekId"] = new SelectList(_context.Set<ForecastWeek>(), "Id", "Id", forecast.ForecastWeekId);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "Contract", forecast.ProjectID);
            return View(forecast);
        }

        // POST: Forecasts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,ForecastWeekId,ProjectID,ForecastType,Hours,Id,CreatedDate,ModifiedDate")] Forecast forecast)
        {
            if (id != forecast.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forecast);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForecastExists(forecast.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "CompanyIdentifier", forecast.EmployeeId);
            ViewData["ForecastWeekId"] = new SelectList(_context.Set<ForecastWeek>(), "Id", "Id", forecast.ForecastWeekId);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "Contract", forecast.ProjectID);
            return View(forecast);
        }

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

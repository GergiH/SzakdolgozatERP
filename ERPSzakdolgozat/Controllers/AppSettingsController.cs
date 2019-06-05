using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Controllers
{
	public class AppSettingsController : MyController
	{
		public AppSettingsController(ERPDBContext context) : base(context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _context.AppSettings.OrderBy(c => c.SettingName).ToListAsync());
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var setting = await _context.AppSettings.FindAsync(id);
			if (setting == null)
			{
				return NotFound();
			}

			SetSettingDropdown(ref setting);
			return View(setting);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, AppSetting setting)
		{
			if (id != setting.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(setting);
					await _context.SaveChangesAsync();

					TempData["Toast"] = Toasts.Saved;

					await LogicAfterSaveAsync(setting);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SettingExists(setting.Id))
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

			SetSettingDropdown(ref setting);
			return View(setting);
		}

		private bool SettingExists(int id)
		{
			return _context.AppSettings.Any(e => e.Id == id);
		}

		private void SetSettingDropdown(ref AppSetting setting)
		{
			// TODO expand the list as settings expand as well
			switch (setting.SettingName)
			{
				case "Default - Currency":
					ViewData["SettingList"] = new SelectList(
						_context.Currencies
							.Where(c => c.InYear == DateTime.Now.Year)
							.Select(c => new SelectListItem
							{
								Value = c.CurrencyName,
								Text = c.CurrencyName
							})
							.OrderBy(c => c.Text)
							.ToList(),
						"Value", "Text", setting.SettingValue);
					break;
				case "Default - Overtime Multiplier":
					double[] multipliers = new double[21];
					for (int i = 0; i < 21; i++)
					{
						multipliers[i] = 1 + (i / 10.0);
					}
					ViewData["SettingList"] = new SelectList(multipliers);
					break;
				default:
					ViewData["SettingList"] = null;
					break;
			}
		}

		public async Task LogicAfterSaveAsync(AppSetting setting)
		{
			switch (setting.SettingName)
			{
				// set the new default currency's multiplier to 1
				case "Default - Currency":
					var curr = _context.Currencies
						.Where(c => c.CurrencyName == setting.SettingValue && c.InYear == DateTime.Now.Year)
						.FirstOrDefault();
					if (curr != null)
					{
						curr.ExchangeValue = 1;

						_context.Update(curr);
						await _context.SaveChangesAsync();
					}
					break;
				default:
					break;
			}
		}
	}
}
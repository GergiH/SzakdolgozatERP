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
		private readonly ERPDBContext _context;

		public AppSettingsController(ERPDBContext context)
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
				case "Default - Currency": // TODO implement logic based on this setting for currency value
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
				default:
					ViewData["SettingList"] = null;
					break;
			}
		}
	}
}
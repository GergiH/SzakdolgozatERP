using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Controllers
{
	[Authorize(Policy = "Admin")]
	public class CurrenciesController : MyController
	{
		public CurrenciesController(ERPDBContext context) : base(context)
		{
			_context = context;
		}

		// GET: Currencies
		public async Task<IActionResult> Index()
		{
			return View(await _context.Currencies.ToListAsync());
		}

		// GET: Currencies/Create
		public IActionResult Create()
		{
			Currency currency = new Currency();
			return View(currency);
		}

		// POST: Currencies/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Currency currency)
		{
			if (ModelState.IsValid)
			{
				currency.CreatedDate = DateTime.Now;
				currency.ModifiedDate = DateTime.Now;
				currency.Id = _context.Currencies.Max(c => c.Id) + 1;

				_context.Add(currency);
				await _context.SaveChangesAsync();

				TempData["Toast"] = Toasts.Created;
				return RedirectToAction(nameof(Index));
			}
			return View(currency);
		}

		// GET: Currencies/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var currency = await _context.Currencies.FindAsync(id);
			if (currency == null)
			{
				return NotFound();
			}
			return View(currency);
		}

		// POST: Currencies/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Currency currency)
		{
			if (id != currency.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(currency);
					await _context.SaveChangesAsync();

					TempData["Toast"] = Toasts.Saved;
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CurrencyExists(currency.Id))
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
			return View(currency);
		}

		// GET: Currencies/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var currency = await _context.Currencies
				.FirstOrDefaultAsync(m => m.Id == id);
			if (currency == null)
			{
				return NotFound();
			}

			return View(currency);
		}

		// POST: Currencies/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var currency = await _context.Currencies.FindAsync(id);
			_context.Currencies.Remove(currency);
			await _context.SaveChangesAsync();

			TempData["Toast"] = Toasts.Deleted;
			return RedirectToAction(nameof(Index));
		}

		private bool CurrencyExists(int id)
		{
			return _context.Currencies.Any(e => e.Id == id);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Mvc;

namespace ERPSzakdolgozat.Controllers
{
    public class AdminController : Controller
    {
		private readonly ERPDBContext _context;

		public AdminController(ERPDBContext context)
		{
			_context = context;
		}

		// TODO Teams index view
		public IActionResult Teams()
        {
            return View();
        }

		public IActionResult Units(string search, bool active = true)
		{
			IEnumerable<Unit> units = _context.Units;

			if (!string.IsNullOrEmpty(search))
			{
				units = units.Where(e => e.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase));
			}

			if (active == true)
			{
				units = units.Where(e => e.Active == true);
			}
			else
			{
				units = units.Where(e => e.Active == false);
			}

			units = units.OrderBy(e => e.Name);

			ViewBag.search = search;
			ViewBag.active = active;

			return View(units);
		}

		public Unit GetUnit(int id)
		{
			return _context.Units.Find(id);
		}

		public int SaveUnit(int id, string code, string name, bool active)
		{
			Unit unit = _context.Units.Find(id);

			unit.Code = code;
			unit.Name = name;
			unit.Active = active;
			unit.ModifiedDate = DateTime.Now;

			_context.SaveChanges();

			return 0;
		}

		public IActionResult CreateUnit(string createCode, string createName)
		{
			Unit unit = new Unit
			{
				Id = _context.Units.Max(u => u.Id) + 1,
				Active = true,
				Code = createCode,
				CreatedDate = DateTime.Now,
				ModifiedDate = DateTime.Now,
				Name = createName
			};

			_context.Units.Add(unit);
			_context.SaveChanges();

			return RedirectToAction("Units");
		}
	}
}
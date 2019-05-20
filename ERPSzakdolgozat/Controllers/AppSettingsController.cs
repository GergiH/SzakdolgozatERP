using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ERPSzakdolgozat.Controllers
{
    public class AppSettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
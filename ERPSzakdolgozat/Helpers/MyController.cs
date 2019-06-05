using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Helpers
{
	public class MyController : Controller
	{
		protected ERPDBContext _context;

		public MyController(ERPDBContext context) : base()
		{
			_context = context;
		}

		protected static class Toasts
		{
			public static string Created = "created-success";
			public static string CreatedFail = "created-fail";
			public static string Saved = "saved-success";
			public static string SavedFail = "saved-fail";
			public static string Deleted = "deleted-success";
			public static string DeletedFail = "deleted-fail";
		}
	}
}
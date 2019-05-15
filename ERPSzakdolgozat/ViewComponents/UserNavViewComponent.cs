using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.ViewComponents
{
	[ViewComponent(Name = "UserNav")]
	public class UserNavViewComponent : ViewComponent
	{
		private readonly ERPDBContext _context;

		public UserNavViewComponent(ERPDBContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync(string username)
		{
			AppUser user = GetUser(username);
			return View("UserNav", user);
		}

		private AppUser GetUser(string username)
		{
			return _context.AppUsers.Where(u => u.ADName == username).FirstOrDefault();
		}
	}
}

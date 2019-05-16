using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			AppUser user = await GetUserAsync(username);
			return View("UserNav", user);
		}

		private async Task<AppUser> GetUserAsync(string username)
		{
			return await _context.AppUsers.Where(u => u.ADName == username).FirstOrDefaultAsync();
		}
	}
}
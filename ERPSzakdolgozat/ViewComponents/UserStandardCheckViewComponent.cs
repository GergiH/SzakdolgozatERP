using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.ViewComponents
{
	[ViewComponent(Name = "UserStandardCheck")]
	public class UserStandardCheckViewComponent : ViewComponent
	{
		private readonly ERPDBContext _context;

		public UserStandardCheckViewComponent(ERPDBContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync(string username)
		{
			AppUser user = await GetUserAsync(username);
			Globals.IsAppUser = user == null ? false : true;
			ViewData["AdministratorNames"] = _context.AppUsers
				.Where(u => u.Roles.Any(r => r.RoleID == 1))
				.Select(u => u.DisplayName)
				.ToList();

			return View("UserStandardCheck", user);
		}

		private async Task<AppUser> GetUserAsync(string username)
		{
			return await _context.AppUsers.Where(u => u.ADName == username).FirstOrDefaultAsync();
		}
	}
}
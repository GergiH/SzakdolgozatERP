using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			AppUser user = await GetUserAsync(username);
			user.Roles = await _context.UserRoles.Where(u => u.UserID == user.Id).ToListAsync();

			if (user.Roles.Any(r => r.RoleID == 1) == true)
			{
				List<DeleteRequest> delreqs = await _context.DeleteRequests.Where(d => d.IsFulfilled == false).ToListAsync();

				ViewData["UsersToDelete"] = delreqs.Count;
				
				if (delreqs.Count > 0)
				{
					ViewData["DeleteRequestsToBeFulfilled"] = true;
				}
			}
			ViewData["UserID"] = user.Id;
			ViewData["DeleteRequestAvailable"] = !_context.DeleteRequests.Any(d => d.AppUserId == user.Id);

			return View("UserNav", user);
		}

		private async Task<AppUser> GetUserAsync(string username)
		{
			return await _context.AppUsers.Where(u => u.ADName == username).FirstOrDefaultAsync();
		}
	}
}
using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using ERPSzakdolgozat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Controllers
{
	public class AppUsersController : MyController
	{
		private int _userID = 0;

		public AppUsersController(ERPDBContext context) : base(context)
		{
			_context = context;
		}

		// GET: Users
		[Authorize(Policy = "Admin")]
		public async Task<IActionResult> Index()
		{
			return View(await _context.AppUsers.AsNoTracking().Include("Roles.AppRole").ToListAsync());
		}

		// GET: Users/Details/5
		[Authorize(Policy = "Admin")]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _context.AppUsers
				.Include("Roles.AppRole")
				.FirstOrDefaultAsync(m => m.Id == id);
			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		// GET: Users/Create
		[Authorize(Policy = "Admin")]
		public IActionResult Create()
		{
			AppUser user = new AppUser();
			return View(user);
		}

		// POST: Users/Create
		[HttpPost]
		[Authorize(Policy = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(AppUser user)
		{
			if (ModelState.IsValid)
			{
				user.CreatedDate = DateTime.Now;
				user.ModifiedDate = DateTime.Now;
				user.Id = _context.AppUsers.Max(t => t.Id) + 1;

				_context.Add(user);
				await _context.SaveChangesAsync();

				TempData["Toast"] = Toasts.Created;
				return RedirectToAction(nameof(Index));
			}
			return View(user);
		}

		// GET: Users/Edit/5
		[Authorize(Policy = "Admin")]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var userEditVM = new AppUser_Edit
			{
				AppUser = await _context.AppUsers.Include("Roles.AppRole").FirstOrDefaultAsync(u => u.Id == id),
				HasRole = new List<bool>()
			};

			if (userEditVM.AppUser == null)
			{
				return NotFound();
			}

			// order roles to match ViewData
			userEditVM.AppUser.Roles = userEditVM.AppUser.Roles.OrderBy(r => r.AppRole.RoleName).ToList();

			var allRoles = _context.AppRoles.AsNoTracking().OrderBy(a => a.RoleName).ToList();
			foreach (var role in allRoles)
			{
				if (userEditVM.AppUser.Roles.Any(r => r.RoleID == role.Id))
				{
					userEditVM.HasRole.Add(true);
				}
				else
				{
					userEditVM.HasRole.Add(false);
				}
			}

			ViewDataRoles();
			return View(userEditVM);
		}

		// POST: Users/Edit/5
		[HttpPost]
		[Authorize(Policy = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, AppUser_Edit userEditVM)
		{
			if (id != userEditVM.AppUser.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(userEditVM.AppUser);
					await _context.SaveChangesAsync();

					// handle the change of roles
					List<AppRole> allRoles = await _context.AppRoles.OrderBy(a => a.RoleName).ToListAsync();
					for (int i = 0; i < allRoles.Count; i++)
					{
						bool alreadyHasRole = _context.UserRoles
							.Any(u => u.RoleID == allRoles[i].Id && u.UserID == userEditVM.AppUser.Id);
						if (userEditVM.HasRole[i] && !alreadyHasRole) // add new role
						{
							UserRoles ur = new UserRoles
							{
								RoleID = allRoles[i].Id,
								UserID = userEditVM.AppUser.Id
							};
							await _context.UserRoles.AddAsync(ur);
						}
						else if (!userEditVM.HasRole[i] && alreadyHasRole) // remove unchecked role
						{
							UserRoles ur = new UserRoles
							{
								RoleID = allRoles[i].Id,
								UserID = userEditVM.AppUser.Id
							};
							_context.UserRoles.Remove(ur);
						}
					}
					await _context.SaveChangesAsync();

					TempData["Toast"] = Toasts.Saved;
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!UserExists(userEditVM.AppUser.Id))
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
			return View(userEditVM);
		}

		// GET: Users/Delete/5
		[Authorize(Policy = "Admin")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _context.AppUsers
				.Include("Roles.AppRole")
				.FirstOrDefaultAsync(m => m.Id == id);
			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		// POST: Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[Authorize(Policy = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var user = await _context.AppUsers.FindAsync(id);
			user.Email = null;
			user.Mobile = null;
			_context.Update(user);

			List<UserRoles> roles = await _context.UserRoles.Where(u => u.UserID == user.Id).ToListAsync();
			_context.UserRoles.RemoveRange(roles);

			var delreq = await _context.DeleteRequests.FirstOrDefaultAsync(d => d.AppUserId == id);
			if (delreq != null)
			{
				delreq.IsFulfilled = true;
				_context.Update(delreq);
			}
			await _context.SaveChangesAsync();

			TempData["Toast"] = Toasts.Deleted;
			return RedirectToAction(nameof(Index));
		}

		private bool UserExists(int id)
		{
			return _context.AppUsers.Any(e => e.Id == id);
		}

		// Self User functions
		[HttpGet]
		public async Task<IActionResult> SelfEdit()
		{
			AppUser user = await _context.AppUsers.Where(u => u.ADName == User.Identity.Name).FirstOrDefaultAsync();

			if (user == null)
			{
				return NotFound();
			}

			ViewDataRoles();
			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> SelfEdit(AppUser user)
		{
			if (ModelState.IsValid)
			{
				try
				{
					// Handling profile picture upload
					IFormFile newPic = Request.Form.Files["profile"];
					if (newPic != null)
					{
						if (newPic.FileName.Contains(".jpg")
							|| newPic.FileName.Contains(".png")
							|| newPic.FileName.Contains(".gif")
							|| newPic.FileName.Contains(".jpeg"))
						{
							using (var memoryStream = new MemoryStream())
							{
								await newPic.CopyToAsync(memoryStream);
								user.ProfilePicture = memoryStream.ToArray();
							}
						}
					}

					_context.Update(user);
					await _context.SaveChangesAsync();

					TempData["Toast"] = Toasts.Saved;
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!UserExists(user.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction("SelfEdit");
			}

			ViewDataRoles();
			return View(user);
		}

		private void ViewDataRoles()
		{
			_userID = _context.AppUsers
				.Where(u => u.ADName == User.Identity.Name)
				.Select(u => u.Id)
				.FirstOrDefault();
			ViewData["UserRoles"] = _context.AppRoles
				.AsNoTracking()
				.Select(r => new
				{
					r.RoleName,
					HasRole = _context.UserRoles.Any(u => u.RoleID == r.Id && u.UserID == _userID)
				})
				.OrderBy(r => r.RoleName)
				.ToDictionary(r => r.RoleName, r => r.HasRole);
		}

		/// <summary>
		/// For resetting the User's picture if it is inappropriate
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<IActionResult> ResetPicture(int id, string viewName)
		{
			AppUser user = await _context.AppUsers.FindAsync(id);

			if (user != null)
			{
				user.ProfilePicture = null;

				_context.Update(user);
				await _context.SaveChangesAsync();

				TempData["Toast"] = Toasts.Saved;
			}
			else
			{
				return NotFound();
			}

			if (viewName == "SelfEdit")
			{
				return RedirectToAction("SelfEdit");
			}

			return RedirectToAction("Edit", new { id });
		}

		public async Task<IActionResult> RequestDelete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var user = await _context.AppUsers
				.FirstOrDefaultAsync(m => m.Id == id);

			if (user == null)
			{
				return NotFound();
			}
			else if (user.ADName != User.Identity.Name)
			{
				return BadRequest();
			}

			return View(user);
		}

		public async Task<IActionResult> RequestDeleteConfirmed(int id)
		{
			if (_context.DeleteRequests.Any(d => d.AppUserId == id) == false)
			{
				DeleteRequest delreq = new DeleteRequest
				{
					Id = _context.DeleteRequests.Max(d => d.Id) + 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					AppUserId = id,
					IsFulfilled = false
				};

				await _context.DeleteRequests.AddAsync(delreq);
				await _context.SaveChangesAsync();

				TempData["Toast"] = Toasts.Created;
			}

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> FulfillDelete()
		{
			List<int> userIds = await _context.DeleteRequests
				.Where(d => d.IsFulfilled == false).Select(d => d.AppUserId).ToListAsync();
			List<AppUser> usersToDelete = new List<AppUser>();

			foreach (var item in userIds)
				usersToDelete.Add(_context.AppUsers.Where(u => u.Id == item).FirstOrDefault());

			return View(usersToDelete);
		}
	}
}
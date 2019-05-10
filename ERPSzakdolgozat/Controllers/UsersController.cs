//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ERPSzakdolgozat.Entities;
//using ERPSzakdolgozat.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace ERPSzakdolgozat.Controllers
//{
//	[Authorize]
//	[ApiController]
//	[Route("[controller]")]
//	public class UsersController : ControllerBase
//    {
//		private IUserService _userService;

//		public UsersController(IUserService userService)
//		{
//			_userService = userService;
//		}

//		[HttpGet("{id}")]
//		public IActionResult GetAll()
//		{
//			var users = _userService.GetAll();
//			return Ok(users);
//		}

//		[HttpGet("{id}")]
//		public IActionResult GetById(int id)
//		{
//			var user = _userService.GetById(id);

//			if (user == null)
//			{
//				return NotFound();
//			}

//			var currentUserName = User.Identity.Name;
//			User.IsInRole
//			if (user.UserName != currentUserName && !User.IsInRole(Role.Admin))
//			{
//				return Forbid();
//			}

//			return Ok(user);
//		}
//	}
//}
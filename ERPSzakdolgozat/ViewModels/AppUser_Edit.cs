using ERPSzakdolgozat.Models;
using System.Collections.Generic;

namespace ERPSzakdolgozat.ViewModels
{
	public class AppUser_Edit
	{
		public AppUser AppUser { get; set; }

		public List<bool> HasRole { get; set; }
	}
}
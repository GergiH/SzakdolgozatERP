using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Models
{
	public class AppUser : BaseProperties
	{
		public string ADName { get; set; }
		public string DisplayName { get; set; }
		public string Email { get; set; }
	}
}

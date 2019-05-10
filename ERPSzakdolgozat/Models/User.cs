using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Models
{
	public class User : BaseProperties
	{
		public string ADName { get; set; }
		public string DisplayName { get; set; }
		public string Email { get; set; }
		public IList<Role> Roles { get; set; } = new List<Role>();
	}
}

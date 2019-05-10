using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Models
{
	public class UserRoles
	{
		[ForeignKey("User")]
		public string ADName { get; set; }
		public virtual User User { get; set; }
		[ForeignKey("AppRole")]
		public string RoleName { get; set; }
		public virtual AppRole AppRole { get; set; }
	}
}

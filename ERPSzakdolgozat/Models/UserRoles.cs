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
		[Key]
		[Column(Order = 0)]
		[ForeignKey("AppUser")]
		public int UserID { get; set; }
		public virtual AppUser AppUser { get; set; }
		[Key]
		[Column(Order = 1)]
		[ForeignKey("AppRole")]
		public int RoleID { get; set; }
		public virtual AppRole AppRole { get; set; }
	}
}

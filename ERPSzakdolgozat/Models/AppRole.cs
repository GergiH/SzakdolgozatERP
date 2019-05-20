using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSzakdolgozat.Models
{
	public class AppRole : BaseProperties
	{
		[Required]
		public string RoleName { get; set; }

		[ForeignKey("Id")]
		public List<UserRoles> UserRoles { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Models
{
	public class AppUser : BaseProperties
	{
		[Required]
		public string ADName { get; set; }
		[Required]
		public string DisplayName { get; set; }
		public string Email { get; set; }
		[Column(TypeName = "image")]
		public byte[] ProfilePicture { get; set; }
	}
}

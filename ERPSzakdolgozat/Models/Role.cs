using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Models
{
	public class Role : BaseProperties
	{
		[Required]
		public string RoleName { get; set; }
		public bool IsSelectable { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Models
{
	public class SkillLevel : BaseProperties
	{
		[Required]
		public string Name { get; set; }
		public bool IsSelectable { get; set; }
	}
}

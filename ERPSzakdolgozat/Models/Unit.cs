using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERPSzakdolgozat.Models
{
	public class Unit : BaseProperties
	{
		[Required]
		public string UnitCode { get; set; }

		[Required]
		public string UnitName { get; set; }

		public bool Active { get; set; }

		public List<Team> Teams { get; set; }
	}
}
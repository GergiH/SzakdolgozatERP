using System.ComponentModel.DataAnnotations;

namespace ERPSzakdolgozat.Models
{
	public class Unit : BaseProperties
	{
		[Required]
		public string Code { get; set; }

		[Required]
		public string Name { get; set; }

		public bool Active { get; set; }
	}
}
using System.ComponentModel.DataAnnotations;

namespace ERPSzakdolgozat.Models
{
	public class Team : BaseProperties
	{
		[Required]
		public string Code { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public int UnitId { get; set; }

		public bool Active { get; set; }
	}
}
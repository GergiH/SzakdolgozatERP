using System.ComponentModel.DataAnnotations;

namespace ERPSzakdolgozat.Models
{
	public class Team : BaseProperties
	{
		[Required]
		public string TeamCode { get; set; }

		[Required]
		public string TeamName { get; set; }

		[Required]
		public int UnitId { get; set; }

		public bool Active { get; set; }
		
		public virtual Unit Unit { get; set; }
	}
}
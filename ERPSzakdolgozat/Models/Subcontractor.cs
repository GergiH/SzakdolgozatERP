using System.ComponentModel.DataAnnotations;

namespace ERPSzakdolgozat.Models
{
	public class Subcontractor : BaseProperties
	{
		[Required]
		public string SubcontractorName { get; set; }
		public bool IsActive { get; set; }
	}
}
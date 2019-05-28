using System.ComponentModel.DataAnnotations;

namespace ERPSzakdolgozat.Models
{
	public class Risk : BaseProperties
	{
		[Required]
		public string RiskName { get; set; }
		public int RiskWeight { get; set; }
	}
}
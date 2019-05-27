using System.ComponentModel.DataAnnotations;

namespace ERPSzakdolgozat.Models
{
	public class AppSetting : BaseProperties
	{
		[Required]
		public string SettingName { get; set; }

		[Required]
		public string SettingValue { get; set; }
	}
}
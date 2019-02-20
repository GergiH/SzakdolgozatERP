using System.ComponentModel.DataAnnotations;

namespace ERPSzakdolgozat.Models
{
	public class Client : BaseProperties
	{
		[Required]
		public string ClientId { get; set; }

		[Required]
		public string ClientName { get; set; }

		public bool Active { get; set; }
	}
}
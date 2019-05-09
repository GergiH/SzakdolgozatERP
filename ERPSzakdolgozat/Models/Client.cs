using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSzakdolgozat.Models
{
	public class Client : BaseProperties
	{
		[Required]
		public string ClientId { get; set; }

		[Required]
		public string ClientName { get; set; }

		public string Email { get; set; }

		public string Phone { get; set; }

		public string Mobile { get; set; }

		public string ContactName { get; set; }

		public string Country { get; set; }

		public string ZIP { get; set; }

		public string City { get; set; }

		public string Street { get; set; }

		[NotMapped] // this means the field is not mapped into a table-field in the DB
		public string Address {
			get => Country + " - " + ZIP + " " + City + ", " + Street;
			set { }
		}

		public string TaxNumber { get; set; }

		public bool Active { get; set; }
	}
}
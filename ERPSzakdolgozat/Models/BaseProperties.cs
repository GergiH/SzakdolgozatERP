using System;
using System.ComponentModel.DataAnnotations;

namespace ERPSzakdolgozat.Models
{
	public class BaseProperties
	{
		[Key]
		[Required]
		public int Id { get; set; }

		[Required]
		public DateTime CreatedDate { get; set; }

		[Required]
		public DateTime ModifiedDate { get; set; }
	}
}
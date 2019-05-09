using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSzakdolgozat.Models
{
	public class BaseProperties
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime CreatedDate { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime ModifiedDate { get; set; } = DateTime.Now;
	}
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSzakdolgozat.Models
{
	public class EmployeeFinancial : BaseProperties
	{
		[ForeignKey("EmployeeId")]
		public Employee Employee { get; set; }
		public int EmployeeId { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime ActiveFrom { get; set; }
		public double GrossSalary { get; set; }
		public double Bonus { get; set; }
		public double Cafeteria { get; set; }
		public int WorkHours { get; set; }
		public int CurrencyId { get; set; }
	}
}
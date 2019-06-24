using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSzakdolgozat.Models
{
	public class WorkDay : BaseProperties
	{
		[Required]
		[DataType(DataType.Date)]
		public DateTime WorkDayDate { get; set; }
		public string WorkDayName { get; set; }
		public bool IsHoliday { get; set; }
		public string Comment { get; set; }
	}
}
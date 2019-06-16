using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSzakdolgozat.Models
{
	public class Forecast : BaseProperties // TODO make controller, seeding, add to dbcontext, views... maybe chart
	{
		[Required]
		[ForeignKey("Employee")]
		public int EmployeeId { get; set; }
		public virtual Employee Employee { get; set; }
		[ForeignKey("ForecastWeek")]
		public int ForecastWeekId { get; set; }
		public virtual ForecastWeek ForecastWeek { get; set; }
		[ForeignKey("Project")]
		public int? ProjectID { get; set; }
		public virtual Project Project { get; set; }

		public string ForecastType { get; set; }

		public string LeaveName { get; set; }

		public double Hours { get; set; }

		public string Comment { get; set; }
	}
}
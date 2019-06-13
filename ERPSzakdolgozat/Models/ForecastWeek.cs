using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSzakdolgozat.Models
{
	public class ForecastWeek : BaseProperties
	{
		[Required]
		[ForeignKey("Employee")]
		public int EmployeeId { get; set; }
		public virtual Employee Employee { get; set; }
		[DataType(DataType.Date)]
		public DateTime WeekStart { get; set; }
		public int WeekNumber { get; set; }
		public double ProjectHours { get; set; }
		public double VacationHours { get; set; }
		public double SicknessHours { get; set; }
		public double TrainingHours { get; set; }
		public double BenchHours { get; set; }
		public double HoursAvailable { get; set; }
		public double HoursAll {
			get {
				return ProjectHours + VacationHours + SicknessHours + TrainingHours + BenchHours;
			}
			set { }
		}

		public List<Forecast> Forecasts { get; set; }
	}
}
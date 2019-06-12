using ERPSzakdolgozat.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSzakdolgozat.Models
{
	public class ProjectResource : BaseProperties
	{
		[Required]
		[ForeignKey("Project")]
		public int ProjectId { get; set; }
		public virtual Project Project { get; set; }
		public string ResourceTask { get; set; }
		[ForeignKey("Employee")]
		public int? ResourceEmployee { get; set; }
		public virtual Employee Employee { get; set; }
		[ForeignKey("Subcontractor")]
		public int? ResourceSubcontractor { get; set; }
		public virtual Subcontractor Subcontractor { get; set; }
		public string ResourceName { get; set; }
		public double HoursAll {
			get {
				return HoursDone + HoursRemaining;
			}
			set { }
		}
		public double OvertimeAll {
			get {
				return OvertimeDone + OvertimeRemaining;
			}
			set { }
		}
		public double HoursDone { get; set; }
		public double OvertimeDone { get; set; }
		public double HoursRemaining { get; set; }
		public double OvertimeRemaining { get; set; }
		public double Cost { get; set; }
		public double Revenue {
			get {
				return Globals.CalculateRevenue(HoursAll, OvertimeAll, Cost);
			}
			set { }
		}
	}
}
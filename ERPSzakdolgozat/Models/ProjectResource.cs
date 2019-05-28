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
		[Required]
		public string Task { get; set; }
		[Required]
		public string ResourceName { get; set; }
		[Required]
		public string ResourceType { get; set; }
		public double HoursAll { get; set; }
		public double OvertimeAll { get; set; }
		public double HoursDone { get; set; }
		public double OvertimeDone { get; set; }
		public double HoursRemaining { get; set; }
		public double OvertimeRemaining { get; set; }
		public double Cost { get; set; }
		public double Revenue { get; set; }
	}
}
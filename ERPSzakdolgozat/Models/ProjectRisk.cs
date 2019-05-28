using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSzakdolgozat.Models
{
	public class ProjectRisk : BaseProperties
	{
		[Required]
		[ForeignKey("Project")]
		public int ProjectId { get; set; }
		public virtual Project Project { get; set; }
		[ForeignKey("Risk")]
		public int RiskId { get; set; }
		public virtual Risk Risk { get; set; }
		public bool IsSelected { get; set; }
	}
}
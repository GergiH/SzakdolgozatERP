using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSzakdolgozat.Models
{
	public class ProjectLog : BaseProperties
	{
		[Required]
		[ForeignKey("Project")]
		public int ProjectId { get; set; }
		public virtual Project Project { get; set; }
		[Required]
		public string FieldName { get; set; }
		public string OriginalValue { get; set; }
		public string NewValue { get; set; }
	}
}
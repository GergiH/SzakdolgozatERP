using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.ViewModels
{
	public class Project_Edit
	{
		public Project Project { get; set; }
		public ProjectResource NewProjectResource { get; set; }
		public SelectList EmployeeSelectList { get; set; }
		public SelectList SubSelectList { get; set; }
	}
}

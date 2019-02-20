using ERPSzakdolgozat.Models;

namespace ERPSzakdolgozat.ViewModels
{
	public class Employee_Index : BaseProperties
	{
		public string EmployeeName { get; set; }

		public bool Active { get; set; }

		public string CompanyIdentifier { get; set; }

		public string LeaderName { get; set; }

		public string TeamName { get; set; }

		public string Address { get; set; }

		public string Mobile { get; set; }

		public string Email { get; set; }
	}
}
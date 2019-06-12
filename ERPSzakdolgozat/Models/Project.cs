using ERPSzakdolgozat.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERPSzakdolgozat.Models
{
	public class Project : BaseProperties
	{
		[Required]
		public string ProjectName { get; set; }

		public string CustomId { get; set; }
		[ForeignKey("Currency")]
		public int CurrencyId { get; set; }
		public Currency Currency { get; set; }

		[Required]
		public string Status { get; set; }

		[Required]
		public string Type { get; set; }

		[Required]
		public string Contract { get; set; }

		[Required]
		public string ProjectManager { get; set; }

		public string Description { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime StartDate { get; set; }

		[DataType(DataType.Date)]
		public DateTime? EndDate { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime EstimatedEndDate { get; set; }

		public double ContractValue { get; set; }
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
		public double ResourcesCost {
			get {
				return ResourcesCostSpent + ResourcesCostRemaining;
			}
			set { }
		}
		public double ResourcesRevenueGained {
			get {
				return Globals.CalculateRevenue(HoursDone, OvertimeDone, TotalCostSpent);
			}
			set { }
		}
		public double ResourcesRevenue {
			get {
				return Globals.CalculateRevenue(HoursAll, OvertimeAll, TotalCost);
			}
			set { }
		}
		public double ResourcesCostSpent { get; set; }
		public double ResourcesCostRemaining { get; set; }
		public double RiskCost {
			get {
				return RiskCostSpent + RiskCostRemaining;
			}
			set { }
		}
		public double RiskRevenue { get; set; }
		public double RiskCostSpent { get; set; }
		public double RiskCostRemaining { get; set; }
		public double TotalCost {
			get {
				return ResourcesCost + RiskCost;
			}
			set { }
		}
		public double TotalRevenue {
			get {
				return ResourcesRevenue + RiskRevenue;
			}
			set { }
		}
		public double TotalCostSpent {
			get {
				return ResourcesCostSpent + RiskCostSpent;
			}
			set { }
		}
		public double TotalCostRemaining {
			get {
				return ResourcesCostRemaining + RiskCostRemaining;
			}
			set { }
		}

		[ForeignKey("Client")]
		public int ClientId { get; set; }

		public virtual Client Client { get; set; }

		public List<ProjectResource> Resources { get; set; }
		public List<ProjectLog> Logs { get; set; }
		public List<ProjectRisk> Risks { get; set; }
	}
}
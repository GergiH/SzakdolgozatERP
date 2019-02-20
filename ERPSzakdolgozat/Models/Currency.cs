using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Models
{
	public class Currency : BaseProperties
	{
		[Required]
		public string CurrencyName { get; set; }
		public double ExchangeValue { get; set; }
		public int InYear { get; set; }
	}
}

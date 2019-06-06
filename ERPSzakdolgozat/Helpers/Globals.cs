using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Helpers
{
	public static class Globals
	{
		//public static bool IsAppUser = false;
		public static double CalculateRevenue(double hours, double overtime, double cost)
		{
			return hours * cost + overtime * cost * 1.5;
		}
	}
}

using ERPSzakdolgozat.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Helpers
{
	public static class Globals
	{
		public static Dictionary<string, string> UserNames;

		public static string GenerateRandomString(int len)
		{
			string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			Random random = new Random();
			char[] stringChars = new char[len];
			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}

			return new String(stringChars);
		}

		public static int GenerateRandomNumber(int min, int max)
		{
			Random random = new Random();
			return random.Next(min, max);
		}

		public static double CalculateRevenue(double hours, double overtime, double cost)
		{
			return hours * cost + overtime * cost * 1.5;
		}

		public static int GetCurrentWeekNumber()
		{
			CultureInfo culture = CultureInfo.CurrentCulture;
			return culture.Calendar.GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
		}
	}
}

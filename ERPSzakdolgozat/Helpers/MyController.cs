using Microsoft.AspNetCore.Mvc;

namespace ERPSzakdolgozat.Helpers
{
	public class MyController : Controller
	{
		protected static class Toasts
		{
			public static string Created = "created-success";
			public static string CreatedFail = "created-fail";
			public static string Saved = "saved-success";
			public static string SavedFail = "saved-fail";
			public static string Deleted = "deleted-success";
			public static string DeletedFail = "deleted-fail";
		}
	}
}
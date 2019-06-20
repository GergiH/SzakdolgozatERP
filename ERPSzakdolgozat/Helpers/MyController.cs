using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Helpers
{
	public class MyController : Controller
	{
		protected ERPDBContext _context;

		public MyController(ERPDBContext context) : base()
		{
			_context = context;
		}

		protected static class Toasts
		{
			public static string Created = "created-success";
			public static string CreatedFail = "created-fail";
			public static string Saved = "saved-success";
			public static string SavedFail = "saved-fail";
			public static string Deleted = "deleted-success";
			public static string DeletedFail = "deleted-fail";
		}

		public async Task ProjectLogCheck<T>(int projId, T originalValue, T newValue, string field)
		{
			// needed the double check because of strings
			if (originalValue != null && newValue != null)
			{
				if (!EqualityComparer<T>.Default.Equals(originalValue, newValue))
				{
					ProjectLog log = new ProjectLog
					{
						CreatedDate = DateTime.Now,
						FieldName = field,
						Id = _context.ProjectLogs.Max(l => l.Id) + 1,
						ModifiedDate = DateTime.Now,
						NewValue = newValue.ToString(),
						OriginalValue = originalValue.ToString(),
						ProjectId = projId,
						UserId = await _context.AppUsers.Where(u => u.ADName == User.Identity.Name).Select(u => u.Id).FirstOrDefaultAsync()
					};
					await _context.ProjectLogs.AddAsync(log);
					await _context.SaveChangesAsync();
				}
			}
			else if (originalValue == null)
			{
				ProjectLog log = new ProjectLog
				{
					CreatedDate = DateTime.Now,
					FieldName = field,
					Id = _context.ProjectLogs.Max(l => l.Id) + 1,
					ModifiedDate = DateTime.Now,
					NewValue = newValue.ToString(),
					OriginalValue = null,
					ProjectId = projId,
					UserId = await _context.AppUsers.Where(u => u.ADName == User.Identity.Name).Select(u => u.Id).FirstOrDefaultAsync()
				};
				await _context.ProjectLogs.AddAsync(log);
				await _context.SaveChangesAsync();
			}
			else if (newValue == null)
			{
				ProjectLog log = new ProjectLog
				{
					CreatedDate = DateTime.Now,
					FieldName = field,
					Id = _context.ProjectLogs.Max(l => l.Id) + 1,
					ModifiedDate = DateTime.Now,
					NewValue = null,
					OriginalValue = originalValue.ToString(),
					ProjectId = projId,
					UserId = await _context.AppUsers.Where(u => u.ADName == User.Identity.Name).Select(u => u.Id).FirstOrDefaultAsync()
				};
				await _context.ProjectLogs.AddAsync(log);
				await _context.SaveChangesAsync();
			}
		}

		public async Task ProjectLogCheck<T>(int projId, T? originalValue, T? newValue, string field) where T : struct
		{
			if (originalValue == null)
			{
				ProjectLog log = new ProjectLog
				{
					CreatedDate = DateTime.Now,
					FieldName = field,
					Id = _context.ProjectLogs.Max(l => l.Id) + 1,
					ModifiedDate = DateTime.Now,
					NewValue = newValue.ToString(),
					OriginalValue = null,
					ProjectId = projId,
					UserId = await _context.AppUsers.Where(u => u.ADName == User.Identity.Name).Select(u => u.Id).FirstOrDefaultAsync()
				};
				await _context.ProjectLogs.AddAsync(log);
				await _context.SaveChangesAsync();
			}
			else if (newValue == null)
			{
				ProjectLog log = new ProjectLog
				{
					CreatedDate = DateTime.Now,
					FieldName = field,
					Id = _context.ProjectLogs.Max(l => l.Id) + 1,
					ModifiedDate = DateTime.Now,
					NewValue = null,
					OriginalValue = originalValue.ToString(),
					ProjectId = projId,
					UserId = await _context.AppUsers.Where(u => u.ADName == User.Identity.Name).Select(u => u.Id).FirstOrDefaultAsync()
				};
				await _context.ProjectLogs.AddAsync(log);
				await _context.SaveChangesAsync();
			}
		}
	}
}
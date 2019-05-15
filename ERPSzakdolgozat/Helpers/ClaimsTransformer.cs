using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERPSzakdolgozat.Helpers
{
	public class ClaimsTransformer : IClaimsTransformation
	{
		private readonly ERPDBContext _context;

		public ClaimsTransformer(ERPDBContext context)
		{
			_context = context;
		}

		public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
		{
			CheckRoles(ref principal);
			return Task.FromResult(principal);
		}

		private void CheckRoles(ref ClaimsPrincipal principal)
		{
			ClaimsIdentity ci = (ClaimsIdentity)principal.Identity;

			// Get all roles of the User
			string[] userRoles = _context.UserRoles.Where(u => u.User.ADName == ci.Name).Select(r => r.AppRole.RoleName).ToArray();
			List<string> actualRoles = ci.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
			foreach (string role in userRoles)
			{
				if (!ci.HasClaim(ClaimTypes.Role, role))
				{
					ci.AddClaim(new Claim(ClaimTypes.Role, role));
				}

				actualRoles.Remove(role);
			}

			// If User has more roles than they should, remove them (...I mean the roles, you dummy!)
			if (actualRoles.Count > 0)
			{
				foreach (string role in actualRoles)
				{
					ci.RemoveClaim(new Claim(ClaimTypes.Role, role));
				}
			}
		}
	}
}

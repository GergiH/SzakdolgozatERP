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
		public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
		{
			//add new claim
			var ci = (ClaimsIdentity)principal.Identity;
			var c = new Claim(ClaimTypes.Role, "Admin");
			ci.AddClaim(c);
			return Task.FromResult(principal);
		}
	}
}

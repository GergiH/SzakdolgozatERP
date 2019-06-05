using ERPSzakdolgozat.Helpers;
using ERPSzakdolgozat.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;

namespace ERPSzakdolgozat
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => false;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			// To use in-memory Database for better demoing
			services.AddDbContext<ERPDBContext>(opt => opt.UseInMemoryDatabase("ERPDB"));

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// TODO dont know how this works... need to change options to something
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "/auth/login";
					options.AccessDeniedPath = "/auth/accessdenied";
				});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
				options.AddPolicy("HR", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "HR"));
				options.AddPolicy("ProjectManager", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "ProjectManager"));
				options.AddPolicy("Sales", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "Sales"));
				options.AddPolicy("Assisstant", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "Assisstant"));
			});

			services.AddScoped<IClaimsTransformation, ClaimsTransformer>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			// For seeding the database
			var context = serviceProvider.GetService<ERPDBContext>();
			context.Database.EnsureCreated();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
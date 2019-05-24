using Microsoft.EntityFrameworkCore;
using System;

namespace ERPSzakdolgozat.Models
{
	public class ERPDBContext : DbContext
	{
		public ERPDBContext(DbContextOptions<ERPDBContext> options) : base(options)
		{
		}

		public DbSet<Employee> Employees { get; set; }
		public DbSet<EmployeeFinancial> EmployeeFinancials { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<Unit> Units { get; set; }
		public DbSet<SkillLevel> SkillLevels { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Currency> Currencies { get; set; }
		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<AppRole> AppRoles { get; set; }
		public DbSet<UserRoles> UserRoles { get; set; }
		public DbSet<Client> Client { get; set; }

		// Seeding the DB
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AppUser>().HasData(
				new AppUser
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					ADName = "CORP\\ghorvath", // Work UserName
											   //ADName = "NyomdNekiNyuszi\\Horváth Gergely", // Home UserName
											   //ADName = User.Identity.Name, // This would be best for debugging, but doesn't work
					Email = "van@denincs.com",
					Mobile = "+36901234567",
					DisplayName = "Horváth Gergely"
				});

			modelBuilder.Entity<AppRole>().HasData(
				new AppRole
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RoleName = "Admin"
				},
				new AppRole
				{
					Id = 2,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RoleName = "HR"
				},
				new AppRole
				{
					Id = 3,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RoleName = "ProjectManager"
				},
				new AppRole
				{
					Id = 4,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RoleName = "Sales"
				},
				new AppRole
				{
					Id = 5,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RoleName = "Assisstant"
				});

			modelBuilder.Entity<UserRoles>()
				.HasKey(u => new { u.UserID, u.RoleID });
			modelBuilder.Entity<UserRoles>().HasData(
				new UserRoles
				{
					UserID = 1,
					RoleID = 1
				});

			// Add at least one record of every Model
			modelBuilder.Entity<Employee>().HasData(
				new Employee
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					Active = true,
					CompanyIdentifier = "XY0001",
					DateOfBirth = new DateTime(1990, 12, 26),
					Email = "kiskutya@corgik.hu",
					LeaderId = 2,
					Mobile = "+36901234567",
					JoinedOn = DateTime.Now.AddYears(-1),
					EmployeeName = "Kis Kutya",
					TeamId = 1,
					SameAddress = false,
					HomeZIP = "2400",
					HomeCountry = "Magyarország",
					HomeCity = "Dunaújváros",
					HomeStreet = "Táncsics M. u. 28.",
					IsLeader = false,
					RoleId = 1
				},
				new Employee
				{
					Id = 2,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					Active = true,
					CompanyIdentifier = "ZZ9999",
					DateOfBirth = new DateTime(1990, 12, 25),
					Email = "nagykutya@husky.hu",
					LeaderId = 2,
					Mobile = "+36909999999",
					JoinedOn = DateTime.Now.AddYears(-1),
					EmployeeName = "Nagy Kutya",
					TeamId = 1,
					SameAddress = true,
					HomeZIP = "2400",
					HomeCountry = "Magyarország",
					HomeCity = "Dunaújváros",
					HomeStreet = "Táncsics M. u. 1/a.",
					MailZIP = "2400",
					MailCountry = "Magyarország",
					MailCity = "Dunaújváros",
					MailStreet = "Táncsics M. u. 1/a.",
					IsLeader = true,
					RoleId = 2
				});

			modelBuilder.Entity<EmployeeFinancial>().HasData(
				new EmployeeFinancial
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					ActiveFrom = DateTime.Now.AddMonths(-2),
					Bonus = 50,
					Cafeteria = 200,
					CurrencyId = 2,
					EmployeeId = 1,
					GrossSalary = 400,
					WorkHours = 8
				},
				new EmployeeFinancial
				{
					Id = 2,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					ActiveFrom = DateTime.Now.AddMonths(-1),
					Bonus = 1000,
					Cafeteria = 0,
					CurrencyId = 2,
					EmployeeId = 1,
					GrossSalary = 10,
					WorkHours = 25
				},
				new EmployeeFinancial
				{
					Id = 3,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					ActiveFrom = DateTime.Now.AddMonths(-1),
					Bonus = 0,
					Cafeteria = 1,
					CurrencyId = 2,
					EmployeeId = 2,
					GrossSalary = 99999,
					WorkHours = 2
				});

			modelBuilder.Entity<Currency>().HasData(
				new Currency
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					ExchangeValue = 1,
					InYear = 2019,
					CurrencyName = "HUF"
				},
				new Currency
				{
					Id = 2,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					ExchangeValue = 300,
					InYear = 2019,
					CurrencyName = "USD"
				});

			modelBuilder.Entity<Role>().HasData(
				new Role
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					IsSelectable = true,
					RoleName = "Developer"
				},
				new Role
				{
					Id = 2,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					IsSelectable = true,
					RoleName = "Tester"
				},
				new Role
				{
					Id = 3,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					IsSelectable = true,
					RoleName = "Manager"
				});

			modelBuilder.Entity<SkillLevel>().HasData(
				new SkillLevel
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					IsSelectable = true,
					SkillLevelName = "Junior"
				},
				new SkillLevel
				{
					Id = 2,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					IsSelectable = true,
					SkillLevelName = "Senior"
				},
				new SkillLevel
				{
					Id = 3,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					IsSelectable = false,
					SkillLevelName = "God"
				});

			modelBuilder.Entity<Team>().HasData(
				new Team
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					Active = true,
					TeamCode = "BTE",
					TeamName = "Best Team Ever",
					UnitId = 1
				});

			modelBuilder.Entity<Unit>().HasData(
				new Unit
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					Active = true,
					UnitCode = "BUE",
					UnitName = "Best Unit Ever"
				});

			modelBuilder.Entity<Client>().HasData(
				new Client
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					Active = true,
					ClientId = "CLIENT001",
					City = "Nagylackunháza",
					ClientName = "Kliens neve",
					ContactName = "Helybenjáró Kelemen",
					Country = "Mexikó",
					Email = "kelemen@itt.hu",
					Mobile = "06901112222",
					Phone = "061987987",
					Street = "Nagymama u. 99.",
					TaxNumber = "554-4444554-222",
					ZIP = "1234"
				},
				new Client
				{
					Id = 2,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					Active = true,
					ClientId = "CLIENT002",
					City = "Kistejföl",
					ClientName = "Nem is a neve",
					ContactName = "Helybenjáró Kelemen",
					Country = "Oroszország",
					Email = "kelemen@itt_is.hu",
					Mobile = "06901112222",
					Phone = "061987988",
					Street = "Papa u. 1.",
					TaxNumber = "52254-4124-2222322",
					ZIP = "4321"
				});

			// Set relations
			modelBuilder.Entity<EmployeeFinancial>()
				.HasOne(e => e.Employee)
				.WithMany(e => e.EmployeeFinancials)
				.HasForeignKey(e => e.EmployeeId);

			modelBuilder.Entity<UserRoles>()
				.HasOne(u => u.AppUser)
				.WithMany(u => u.Roles)
				.HasForeignKey(u => u.UserID);

			modelBuilder.Entity<UserRoles>()
				.HasOne(u => u.AppRole)
				.WithMany(u => u.UserRoles)
				.HasForeignKey(u => u.RoleID);

			//#region PostSeed
			//modelBuilder.Entity<Post>().HasData(
			//	new Post() { BlogId = 1, PostId = 1, Title = "First post", Content = "Test 1" });
			//#endregion

			//#region AnonymousPostSeed
			//modelBuilder.Entity<Post>().HasData(
			//	new { BlogId = 1, PostId = 2, Title = "Second post", Content = "Test 2" });
			//#endregion

			//#region OwnedTypeSeed
			//modelBuilder.Entity<Post>().OwnsOne(p => p.AuthorName).HasData(
			//	new { PostId = 1, First = "Andriy", Last = "Svyryd" },
			//	new { PostId = 2, First = "Diego", Last = "Vega" });
			//#endregion
		}
	}
}
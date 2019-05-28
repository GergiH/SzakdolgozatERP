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
		public DbSet<Client> Clients { get; set; }
		public DbSet<AppSetting> AppSettings { get; set; }
		public DbSet<Risk> Risks { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<ProjectResource> ProjectResources { get; set; }
		public DbSet<ProjectLog> ProjectLogs { get; set; }
		public DbSet<ProjectRisk> ProjectRisks { get; set; }
		public DbSet<Subcontractor> Subcontractors { get; set; }

		// Seeding the DB
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Add at least one record of every Model
			modelBuilder.Entity<AppSetting>().HasData(
				new AppSetting
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					SettingName = "Default - Currency",
					SettingValue = "HUF"
				},
				new AppSetting
				{
					Id = 2,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					SettingName = "Default - Overtime Multiplier",
					SettingValue = "1,4"
				});

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
					TeamCode = "TitBT",
					TeamName = "This is the Best Team",
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

			modelBuilder.Entity<Risk>().HasData( // TODO admin felület risk-nek
				new Risk
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RiskName = "Employee absence",
					RiskWeight = 10
				},
				new Risk
				{
					Id = 2,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RiskName = "Resource shortage",
					RiskWeight = 5
				},
				new Risk
				{
					Id = 3,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RiskName = "Wrong estimation",
					RiskWeight = 2
				},
				new Risk
				{
					Id = 4,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RiskName = "Misunderstanding of specifications",
					RiskWeight = 6
				});

			modelBuilder.Entity<Subcontractor>().HasData( // TODO subcontractoroknak menü (resources alatt employees és subcontractors)
				new Subcontractor
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					SubcontractorName = "Külsős Kálmán",
					IsActive = true
				},
				new Subcontractor
				{
					Id = 2,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					SubcontractorName = "Beépített Benedek",
					IsActive = true
				},
				new Subcontractor
				{
					Id = 3,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					SubcontractorName = "Kiszervezett Kamilla",
					IsActive = true
				});

			modelBuilder.Entity<Project>().HasData( // TODO projekteknek felület és controller hegyek (+ chart.js a log alapján a risk és total costra és revenuera)
				new Project
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					ClientId = 1,
					Contract = "Signed",
					ContractValue = 2800000,
					Description = "Nagyon szuper projekt, ami mindenkinek örömet okoz.",
					EstimatedEndDate = DateTime.Now.AddMonths(6),
					HoursAll = 200,
					HoursDone = 150,
					HoursRemaining = 50,
					OvertimeAll = 20,
					OvertimeDone = 0,
					OvertimeRemaining = 20,
					ProjectManager = "CORP\\ghorvath",
					ProjectName = "Maybe the best project",
					ResourcesCost = 1600000,
					ResourcesRevenue = 2400000,
					ResourcesCostRemaining = 400000,
					ResourcesCostSpent = 1200000,
					RiskCost = 200000,
					RiskCostRemaining = 100000,
					RiskCostSpent = 100000,
					RiskRevenue = 400000,
					StartDate = DateTime.Now,
					Status = "Executing",
					TotalCost = 1800000,
					TotalCostRemaining = 500000,
					TotalCostSpent = 1300000,
					TotalRevenue = 2800000,
					Type = "Fixed price"
				});

			modelBuilder.Entity<ProjectLog>().HasData(
				new ProjectLog
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					FieldName = "HoursDone",
					OriginalValue = "100",
					NewValue = "150",
					ProjectId = 1
				});

			modelBuilder.Entity<ProjectRisk>().HasData(
				new ProjectRisk
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RiskId = 1,
					ProjectId = 1,
					IsSelected = true
				},
				new ProjectRisk
				{
					Id = 2,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RiskId = 2,
					ProjectId = 1,
					IsSelected = true
				},
				new ProjectRisk
				{
					Id = 3,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RiskId = 3,
					ProjectId = 1,
					IsSelected = false
				},
				new ProjectRisk
				{
					Id = 4,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RiskId = 4,
					ProjectId = 1,
					IsSelected = false
				});

			modelBuilder.Entity<ProjectResource>().HasData(
				new ProjectResource
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					Task = "Doesn't do anything",
					ProjectId = 1,
					ResourceName = "Kis Kutya",
					ResourceType = "Employee",
					Cost = 5000,
					HoursAll = 150,
					HoursDone = 100,
					HoursRemaining = 50,
					OvertimeAll = 20,
					OvertimeDone = 0,
					OvertimeRemaining = 20,
					Revenue = 8000
				},
				new ProjectResource
				{
					Id = 2,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					Task = "Pretends he does something",
					ProjectId = 1,
					ResourceName = "Beépített Benedek",
					ResourceType = "Subcontractor",
					Cost = 7000,
					HoursAll = 50,
					HoursDone = 50,
					HoursRemaining = 0,
					OvertimeAll = 0,
					OvertimeDone = 0,
					OvertimeRemaining = 0,
					Revenue = 8000
				}); // TODO ha resourcetype == Other, akkor ne dropdown legyen a View-ban

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
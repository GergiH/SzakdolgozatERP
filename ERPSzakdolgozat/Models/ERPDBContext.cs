using ERPSzakdolgozat.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using ERPSzakdolgozat.Models;
using System.Linq;

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
		public DbSet<Forecast> Forecast { get; set; }
		public DbSet<ForecastWeek> ForecastWeeks { get; set; }

		// Seeding the DB
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
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
					SettingName = "Forecast - Generate Weeks",
					SettingValue = "6"
				});

			modelBuilder.Entity<AppUser>().HasData(
				new AppUser
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					ADName = "CORP\\ghorvath", // Work UserName
											   //ADName = "NYOMDNEKINYUSZI\\Horváth Gergely",
											   //ADName = User.Identity.Name, // This would be best for debugging, but doesn't work
					Email = "van@denincs.com",
					Mobile = "+36901234567",
					DisplayName = "Horváth Gergely"
				},
				new AppUser
				{
					Id = 2,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					ADName = "CORP\\UndefinedUser",
					Email = "van@denincs.com",
					Mobile = "+36901234567",
					DisplayName = "Not Available"
				},
				new AppUser
				{
					Id = 3,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					ADName = "CORP\\Ghost",
					Email = "van@denincs.com",
					Mobile = "+36901234567",
					DisplayName = "Ghost in the Dark"
				});

			// random AppUsers
			for (int i = 0; i < 5; i++)
			{
				modelBuilder.Entity<AppUser>().HasData(
					new AppUser
					{
						Id = 4 + i,
						CreatedDate = DateTime.Now,
						ModifiedDate = DateTime.Now,
						ADName = "CORP\\" + Globals.GenerateRandomString(7),
						Email = Globals.GenerateRandomString(5) + "@provider.address",
						Mobile = "+36901234567",
						DisplayName = Globals.GenerateRandomString(6) + " " + Globals.GenerateRandomString(8)
					});
			}

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
				},
				new AppRole
				{
					Id = 6,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RoleName = "TeamLeader"
				});

			modelBuilder.Entity<UserRoles>()
				.HasKey(u => new { u.UserID, u.RoleID });
			modelBuilder.Entity<UserRoles>().HasData(
				new UserRoles
				{
					UserID = 1,
					RoleID = 1
				},
				new UserRoles
				{
					UserID = 2,
					RoleID = 3
				},
				new UserRoles
				{
					UserID = 2,
					RoleID = 4
				},
				new UserRoles
				{
					UserID = 3,
					RoleID = 2
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

			// random Employees and EmployeeFinancials
			for (int i = 0; i < 20; i++)
			{
				modelBuilder.Entity<Employee>().HasData(
					new Employee
					{
						Id = 3 + i,
						CreatedDate = DateTime.Now,
						ModifiedDate = DateTime.Now,
						Active = i % 2 == 0 ? true : false,
						CompanyIdentifier = Globals.GenerateRandomString(5).ToUpper(),
						DateOfBirth = new DateTime(1990, 12, 26),
						Email = Globals.GenerateRandomString(5) + "@corgik.hu",
						LeaderId = 2,
						Mobile = "+36901234567",
						JoinedOn = DateTime.Now.AddYears(-1),
						EmployeeName = Globals.GenerateRandomString(7) + " " + Globals.GenerateRandomString(5),
						TeamId = i < 10 ? 1 : 2,
						SameAddress = false,
						HomeZIP = "2400",
						HomeCountry = "Magyarország",
						HomeCity = "Dunaújváros",
						HomeStreet = Globals.GenerateRandomString(11) + " u." + (i + 2).ToString() + ".",
						IsLeader = false,
						RoleId = i < 5 ? 1 : i < 10 ? 2 : 3,
						SkillLevelId = i < 5 ? 1 : i < 19 ? 2 : 3
					});

				modelBuilder.Entity<EmployeeFinancial>().HasData(
					new EmployeeFinancial
					{
						Id = 4 + i,
						CreatedDate = DateTime.Now,
						ModifiedDate = DateTime.Now,
						ActiveFrom = DateTime.Now.AddMonths(Globals.GenerateRandomNumber(-4, 4)),
						Bonus = Globals.GenerateRandomNumber(0, 25000),
						Cafeteria = Globals.GenerateRandomNumber(1000, 9000),
						CurrencyId = i < 16 ? 1 : 2,
						EmployeeId = 3 + i,
						GrossSalary = Globals.GenerateRandomNumber(40000, 600000),
						WorkHours = Globals.GenerateRandomNumber(4, 8)
					});
			}

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

			// random Teams and Units
			for (int i = 0; i < 5; i++)
			{
				modelBuilder.Entity<Team>().HasData(
					new Team
					{
						Id = 2 + i,
						CreatedDate = DateTime.Now,
						ModifiedDate = DateTime.Now,
						Active = i < 4 ? true : false,
						TeamCode = Globals.GenerateRandomString(4),
						TeamName = Globals.GenerateRandomString(7) + " Team",
						UnitId = i % 2 == 0 ? 1 : i % 3 == 0 ? 2 : 3
					});

				modelBuilder.Entity<Unit>().HasData(
					new Unit
					{
						Id = 2 + i,
						CreatedDate = DateTime.Now,
						ModifiedDate = DateTime.Now,
						Active = i < 4 ? true : false,
						UnitCode = Globals.GenerateRandomString(3),
						UnitName = Globals.GenerateRandomString(5) + " Unit"
					});
			}

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

			// random Clients
			for (int i = 0; i < 15; i++)
			{
				modelBuilder.Entity<Client>().HasData(
				new Client
				{
					Id = 3 + i,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					Active = i < 11 ? true : false,
					ClientId = Globals.GenerateRandomString(5).ToUpper(),
					City = "Nagykun" + Globals.GenerateRandomString(6),
					ClientName = Globals.GenerateRandomString(4) + " " + Globals.GenerateRandomString(5),
					ContactName = "Helybenjáró " + Globals.GenerateRandomString(6),
					Country = "Mexikó",
					Email = Globals.GenerateRandomString(5) + "@itt.hu",
					Mobile = "06901112222",
					Phone = "061987987",
					Street = "Nagymama u. " + Globals.GenerateRandomNumber(1, 99).ToString() + ".",
					TaxNumber = "554-" + Globals.GenerateRandomNumber(10000, 99999).ToString() + "-222",
					ZIP = Globals.GenerateRandomNumber(1000, 9999).ToString()
				});
			}

			modelBuilder.Entity<Risk>().HasData(
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

			modelBuilder.Entity<Subcontractor>().HasData(
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

			// random Subcontractors
			for (int i = 0; i < 20; i++)
			{
				modelBuilder.Entity<Subcontractor>().HasData(
				new Subcontractor
				{
					Id = 4 + i,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					SubcontractorName = Globals.GenerateRandomString(5) + " " + Globals.GenerateRandomString(6),
					IsActive = true
				});
			}

			modelBuilder.Entity<Project>().HasData(
				new Project
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					ClientId = 1,
					Contract = "Signed",
					ContractValue = 2800000,
					CurrencyId = 2,
					CustomId = "PRJ001",
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
					Type = "Fixed price",
				});

			// random Projects, ProjectRisks, ProjectResources
			for (int i = 0; i < 30; i++)
			{
				modelBuilder.Entity<Project>().HasData(
					new Project
					{
						Id = 2 + i,
						CreatedDate = DateTime.Now,
						ModifiedDate = DateTime.Now.AddMonths(Globals.GenerateRandomNumber(-1, 11)),
						ClientId = i < 10 ? 1 : i < 20 ? 2 : 3,
						Contract = i < 10 ? "Signed" : i < 20 ? "Not started" : "In progress",
						ContractValue = Globals.GenerateRandomNumber(200000, 4500000),
						CurrencyId = 2,
						CustomId = Globals.GenerateRandomString(5).ToUpper(),
						EstimatedEndDate = DateTime.Now.AddMonths(6),
						HoursDone = Globals.GenerateRandomNumber(0, 150),
						HoursRemaining = Globals.GenerateRandomNumber(0, 150),
						//HoursAll = this.HoursDone ,
						//OvertimeAll = Globals.GenerateRandomNumber(0, 100),
						OvertimeDone = Globals.GenerateRandomNumber(0, 50),
						OvertimeRemaining = Globals.GenerateRandomNumber(0, 50),
						ProjectManager = "CORP\\ghorvath",
						ProjectName = i < 10 ? Globals.GenerateRandomString(7) : i < 20 ? Globals.GenerateRandomString(10) : Globals.GenerateRandomString(15),
						ResourcesCost = Globals.GenerateRandomNumber(100000, 3000000),
						ResourcesRevenue = Globals.GenerateRandomNumber(200000, 3500000),
						ResourcesCostRemaining = Globals.GenerateRandomNumber(50000, 1000000),
						ResourcesCostSpent = Globals.GenerateRandomNumber(50000, 1000000),
						//RiskCost = Globals.GenerateRandomNumber(50000, 2000000),
						RiskCostRemaining = Globals.GenerateRandomNumber(0, 50000),
						RiskCostSpent = Globals.GenerateRandomNumber(0, 50000),
						RiskRevenue = Globals.GenerateRandomNumber(0, 200000),
						StartDate = DateTime.Now,
						Status = i < 5 ? "Executing" : i < 10 ? "Not started" : i < 20 ? "Failed" : "Finished",
						//TotalCost = Globals.GenerateRandomNumber(200000, 4500000),
						//TotalCostRemaining = Globals.GenerateRandomNumber(100000, 4000000),
						//TotalCostSpent = Globals.GenerateRandomNumber(100000, 4000000),
						//TotalRevenue = Globals.GenerateRandomNumber(200000, 4500000),
						Type = i < 20 ? "Fixed price" : "Time and material",
					});

				for (int j = 0; j < 4; j++)
				{
					modelBuilder.Entity<ProjectRisk>().HasData(
						new ProjectRisk
						{
							Id = 5 + j + i*4,
							CreatedDate = DateTime.Now,
							ModifiedDate = DateTime.Now,
							RiskId = 1 + j,
							ProjectId = 2 + i,
							IsSelected = j < 2 ? true : false
						});
				}

				for (int k = 0; k < 5; k++)
				{
					modelBuilder.Entity<ProjectResource>().HasData(
						new ProjectResource
						{
							Id = 3 + k + i*5,
							CreatedDate = DateTime.Now,
							ModifiedDate = DateTime.Now,
							ResourceTask = Globals.GenerateRandomString(14),
							ProjectId = 2 + i,
							ResourceEmployee = Globals.GenerateRandomNumber(1, 22),
							ResourceName = Globals.GenerateRandomString(10),
							Cost = Globals.GenerateRandomNumber(800, 6000),
							//HoursAll = Globals.GenerateRandomNumber(10, 100),
							HoursDone = Globals.GenerateRandomNumber(0, 40),
							HoursRemaining = Globals.GenerateRandomNumber(0, 20),
							//OvertimeAll = Globals.GenerateRandomNumber(10, 50),
							OvertimeDone = Globals.GenerateRandomNumber(0, 10),
							OvertimeRemaining = Globals.GenerateRandomNumber(0, 5),
							//Revenue = Globals.GenerateRandomNumber(4000, 25000)
						});
				}
			}

			modelBuilder.Entity<ProjectLog>().HasData(
				new ProjectLog
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					FieldName = "HoursDone",
					OriginalValue = "100",
					NewValue = "150",
					ProjectId = 1,
					UserId = 1
				},
				new ProjectLog
				{
					Id = 2,
					CreatedDate = DateTime.Now.AddMinutes(10),
					ModifiedDate = DateTime.Now.AddMinutes(10),
					FieldName = "Type",
					OriginalValue = "Time and material",
					NewValue = "Fixed price",
					ProjectId = 1,
					UserId = 2
				},
				new ProjectLog
				{
					Id = 3,
					CreatedDate = DateTime.Now.AddMinutes(34),
					ModifiedDate = DateTime.Now.AddMinutes(34),
					FieldName = "HoursRemaining",
					OriginalValue = "88",
					NewValue = "50",
					ProjectId = 1,
					UserId = 1
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
					ResourceTask = "Doesn't do anything",
					ProjectId = 1,
					ResourceEmployee = 1,
					ResourceName = "Kis Kutya",
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
					ResourceTask = "Pretends he does something",
					ProjectId = 1,
					ResourceSubcontractor = 1,
					ResourceName = "Külsős Kálmán",
					Cost = 7000,
					HoursAll = 50,
					HoursDone = 50,
					HoursRemaining = 0,
					OvertimeAll = 0,
					OvertimeDone = 0,
					OvertimeRemaining = 0,
					Revenue = 8000
				});

			modelBuilder.Entity<Forecast>().HasData(
				new Forecast
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					EmployeeId = 1,
					ForecastType = "Project",
					ForecastWeekId = 1,
					Hours = 40,
					ProjectID = 1,
					Comment = "Testing this week"
				});

			modelBuilder.Entity<ForecastWeek>().HasData(
				new ForecastWeek
				{
					Id = 1,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					BenchHours = 0,
					EmployeeId = 1,
					HoursAll = 40,
					HoursAvailable = 40,
					ProjectHours = 40,
					SicknessHours = 0,
					TrainingHours = 0,
					VacationHours = 0,
					WeekNumber = 24,
					WeekStart = new DateTime(2019, 6, 10)
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
		}
	}
}
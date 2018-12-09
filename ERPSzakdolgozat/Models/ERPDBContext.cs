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
		public DbSet<Team> Teams { get; set; }
		public DbSet<Unit> Units { get; set; }

		// Seeding the DB
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Add at least one record of every Model
			modelBuilder.Entity<Employee>().HasData(new Employee
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
				Name = "Kis Kutya",
				TeamId = 1,
				SameAddress = true,
				HomeZIP = "2400",
				HomeCountry = "Magyarország",
				HomeCity = "Dunaújváros",
				HomeStreet = "Táncsics M. u. 28.",
				IsLeader = false
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
				Name = "Nagy Kutya",
				TeamId = 1,
				SameAddress = true,
				HomeZIP = "2400",
				HomeCountry = "Magyarország",
				HomeCity = "Dunaújváros",
				HomeStreet = "Táncsics M. u. 1/a.",
				IsLeader = true
			});

			modelBuilder.Entity<Team>().HasData(new Team
			{
				Id = 1,
				CreatedDate = DateTime.Now,
				ModifiedDate = DateTime.Now,
				Active = true,
				Code = "BTE",
				Name = "Best Team Ever",
				UnitId = 1
			});

			modelBuilder.Entity<Unit>().HasData(new Unit
			{
				Id = 1,
				CreatedDate = DateTime.Now,
				ModifiedDate = DateTime.Now,
				Active = true,
				Code = "BUE",
				Name = "Best Unit Ever"
			});

			//modelBuilder.Entity<Post>(entity =>
			//{
			//	entity.HasOne(d => d.Blog)
			//		.WithMany(p => p.Posts)
			//		.HasForeignKey("BlogId");
			//});

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
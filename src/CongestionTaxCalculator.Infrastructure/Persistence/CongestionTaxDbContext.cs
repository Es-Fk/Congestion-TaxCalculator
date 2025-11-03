using CongestionTaxCalculator.Domain.Common;
using CongestionTaxCalculator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CongestionTaxCalculator.Infrastructure.Persistence
{
	public class CongestionTaxDbContext : DbContext
	{

		#region dbset's
		public DbSet<City> Cities => Set<City>();
		public DbSet<Holiday> Holidays => Set<Holiday>();
		public DbSet<TaxExemptVehicle> TaxExemptVehicles => Set<TaxExemptVehicle>();
		public DbSet<TaxRule> TaxRules => Set<TaxRule>();
		public DbSet<TollPassage> TollPassages => Set<TollPassage>();
		public DbSet<Vehicle> Vehicles => Set<Vehicle>(); 
		#endregion
		
		public CongestionTaxDbContext(DbContextOptions<CongestionTaxDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var currentUser = GetCurrentUserName();

			foreach (var entry in ChangeTracker.Entries())
			{
				if (entry.Entity is not BaseEntity<int>)
					continue;

				switch (entry.State)
				{
					case EntityState.Added:
						if (entry.CurrentValues.Properties.Any(p => p.Name == "CreatedBy"))
							entry.CurrentValues["CreatedBy"] = currentUser ?? string.Empty;
						break;

					case EntityState.Modified:
						if (entry.CurrentValues.Properties.Any(p => p.Name == "ModifiedOn"))
							entry.CurrentValues["ModifiedOn"] = DateTime.UtcNow;

						if (entry.CurrentValues.Properties.Any(p => p.Name == "ModifiedBy"))
							entry.CurrentValues["ModifiedBy"] = currentUser ?? string.Empty;
						break;
				}
			}

			return base.SaveChangesAsync(cancellationToken);
		}

		private static string GetCurrentUserName()
		{
			try
			{
				var name = ClaimsPrincipal.Current?.Identity?.Name;
				if (!string.IsNullOrWhiteSpace(name))
					return name;

				name = Thread.CurrentPrincipal?.Identity?.Name;
				if (!string.IsNullOrWhiteSpace(name))
					return name;

				name = Environment.UserName;
				if (!string.IsNullOrWhiteSpace(name))
					return name;
			}
			catch
			{
			}
			return string.Empty;
		}
	}
}

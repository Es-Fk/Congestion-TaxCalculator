using CongestionTaxCalculator.Domain.CityManagement.Entities;
using CongestionTaxCalculator.Domain.Common;
using CongestionTaxCalculator.Domain.TaxCalculation.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CongestionTaxCalculator.Infrastructure.Persistence.DbContexts
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
	}
}

using CongestionTaxCalculator.Domain.CityManagement.Repositories;
using CongestionTaxCalculator.Domain.TaxCalculation.Repositories;
using CongestionTaxCalculator.Infrastructure.Persistence.DbContexts;
using CongestionTaxCalculator.Infrastructure.Persistence.DbProviders;
using CongestionTaxCalculator.Infrastructure.Persistence.Repositories.CityManagement;
using CongestionTaxCalculator.Infrastructure.Persistence.Repositories.TaxCalculation;
using CongestionTaxCalculator.Infrastructure.Persistence.Repositories.VehicleManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CongestionTaxCalculator.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			var dbProvider = configuration.GetSection("DbProvider").Value;
			var dbPprovider = DbProviderFactory.Create(dbProvider);
			dbPprovider.Configure(services, configuration);

			services.AddScoped<ICityRepository, CityRepository>()
				.AddScoped<ITollPassageRepository, TollPassageRepository>()
				.AddScoped<IVehicleRepository, VehicleRepository>();
			return services;
		}
		public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
		{
			Log.Information($"Database migration started");
			var configuration = app.ApplicationServices.GetService<IConfiguration>();
			var dbProvider = configuration!.GetSection("DbProvider").Value;

			if (dbProvider == "SqlServer")
			{
				using var scope = app.ApplicationServices.CreateScope();
				var dbContext = scope.ServiceProvider.GetRequiredService<CongestionTaxDbContext>();

				try
				{
					dbContext.Database.Migrate();
				}
				catch (Exception ex)
				{
					Log.Information($"Database migration failed: {ex.Message}");
				}
			}

			return app;
		}
	}
}

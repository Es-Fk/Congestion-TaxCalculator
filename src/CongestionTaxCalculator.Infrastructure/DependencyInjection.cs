using CongestionTaxCalculator.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

			if (dbProvider == "SqlServer")
			{
				services.AddDbContext<CongestionTaxDbContext>(options =>
				{
					options.UseSqlServer(configuration.GetConnectionString("Persistence"));
				});
			}
			else if (dbProvider == "InMemory")
			{
				services.AddSingleton<InMemoryDatabaseRoot>();
				services.AddDbContext<CongestionTaxDbContext>((sp, options) =>
				   {
					   var root = sp.GetRequiredService<InMemoryDatabaseRoot>();
					   options.UseInMemoryDatabase("CongestionTaxInMemoryDb", root);
				   });
			}

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

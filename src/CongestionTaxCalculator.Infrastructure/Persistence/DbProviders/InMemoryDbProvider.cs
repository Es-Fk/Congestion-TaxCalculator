using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CongestionTaxCalculator.Infrastructure.Persistence.DbContexts;

namespace CongestionTaxCalculator.Infrastructure.Persistence.DbProviders
{
	public class InMemoryDbProvider : IDbProvider
	{
		public void Configure(IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<InMemoryDatabaseRoot>();
			services.AddDbContext<CongestionTaxDbContext>((sp, options) =>
			{
				var root = sp.GetRequiredService<InMemoryDatabaseRoot>();
				options.UseInMemoryDatabase("CongestionTaxInMemoryDb", root);
				options.EnableSensitiveDataLogging();
				options.EnableDetailedErrors();
				options.AddInterceptors(new AuditableEntitySaveChangesInterceptor());
			});
		}
	}
}

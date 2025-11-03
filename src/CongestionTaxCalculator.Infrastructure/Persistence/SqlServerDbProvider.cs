using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CongestionTaxCalculator.Infrastructure.Persistence
{
	public class SqlServerDbProvider : IDbProvider
	{
		public void Configure(IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<CongestionTaxDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("Persistence"));
				options.EnableSensitiveDataLogging();
				options.AddInterceptors(new AuditableEntitySaveChangesInterceptor());
			});
		}
	}
}

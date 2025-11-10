using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CongestionTaxCalculator.Infrastructure.Persistence.DbProviders
{
	public interface IDbProvider
	{
		void Configure(IServiceCollection services, IConfiguration configuration);
	}
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CongestionTaxCalculator.Infrastructure.Persistence
{
	public interface IDbProvider
	{
		void Configure(IServiceCollection services, IConfiguration configuration);
	}
}

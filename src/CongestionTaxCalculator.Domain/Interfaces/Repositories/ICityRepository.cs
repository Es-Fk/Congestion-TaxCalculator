using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Domain.Interfaces.Repositories
{
	public interface ICityRepository:IRepository<City,int>
	{
		Task<City?> GetByIdAsync(int id, CancellationToken ct = default);
	}
}

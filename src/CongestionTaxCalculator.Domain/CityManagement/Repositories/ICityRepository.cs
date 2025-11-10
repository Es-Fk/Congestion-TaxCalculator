using CongestionTaxCalculator.Domain.CityManagement.Repositories;
using CongestionTaxCalculator.Domain.TaxCalculation.Entities;

namespace CongestionTaxCalculator.Domain.TaxCalculation.Repositories
{
	public interface ICityRepository:IRepository<City,int>
	{
		Task<City?> GetByIdAsync(int id, CancellationToken ct = default);
		Task<City?> GetByNameAsync(string name, CancellationToken ct = default);
	}
}

using CongestionTaxCalculator.Domain.CityManagement.Entities;

namespace CongestionTaxCalculator.Domain.CityManagement.Repositories
{
	public interface ITollPassageRepository:IRepository<TollPassage,Guid>
	{
		Task<IEnumerable<TollPassage>> GetPassagesForVehicleOnDateAsync(Guid vehicleId, DateTime date, CancellationToken ct = default);
		Task AddPassageAsync(TollPassage passage, CancellationToken ct = default);
	}
}

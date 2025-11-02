using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Domain.Interfaces.Repositories
{
	public interface ITollPassageRepository:IRepository<TollPassage,Guid>
	{
		Task<IEnumerable<TollPassage>> GetPassagesForVehicleOnDateAsync(Guid vehicleId, DateTime date, CancellationToken ct = default);
		Task AddPassageAsync(TollPassage passage, CancellationToken ct = default);
	}
}

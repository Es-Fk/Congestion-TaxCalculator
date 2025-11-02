using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Domain.Interfaces.Repositories
{
	public interface IVehicleRepository:IRepository<Vehicle,Guid>
	{
		Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task AddAsync(Vehicle vehicle, CancellationToken ct = default);
	}
}

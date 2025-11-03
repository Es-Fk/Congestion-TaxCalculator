using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Entities.Enums;

namespace CongestionTaxCalculator.Domain.Interfaces.Repositories
{
	public interface IVehicleRepository:IRepository<Vehicle,Guid>
	{
		Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task AddAsync(Vehicle vehicle, CancellationToken ct = default);
		Task<Vehicle?> GetByTypeAsync(VehicleType Type, CancellationToken ct = default);
	}
}

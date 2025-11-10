using CongestionTaxCalculator.Domain.CityManagement.Repositories;
using CongestionTaxCalculator.Domain.TaxCalculation.Entities;
using CongestionTaxCalculator.Domain.VehicleManagement.Entities.Enums;

namespace CongestionTaxCalculator.Domain.TaxCalculation.Repositories
{
	public interface IVehicleRepository:IRepository<Vehicle,Guid>
	{
		Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken ct = default);
		Task AddAsync(Vehicle vehicle, CancellationToken ct = default);
		Task<Vehicle?> GetByTypeAsync(VehicleType Type, CancellationToken ct = default);
	}
}

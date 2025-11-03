using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Interfaces.DomainServices;

namespace CongestionTaxCalculator.Domain.DomainServices
{
	public class VehicleTaxExemptionService : IVehicleTaxExemptionService
	{
		public bool IsTaxExempt(Vehicle vehicle, City city)
		{
			return city.TaxExemptVehicles.Any(e => e.Vehicle.VehicleType == vehicle.VehicleType);
		}
	}
}

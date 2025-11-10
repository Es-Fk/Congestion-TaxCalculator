using CongestionTaxCalculator.Domain.TaxCalculation.Entities;
using CongestionTaxCalculator.Domain.VehicleManagement.Services.Interfaces;

namespace CongestionTaxCalculator.Domain.TaxCalculation.DomainServices
{
	public class VehicleTaxExemptionService : IVehicleTaxExemptionService
	{
		public bool IsTaxExempt(Vehicle vehicle, City city)
		{
			return city.TaxExemptVehicles.Any(e => e.Vehicle.VehicleType == vehicle.VehicleType);
		}
	}
}

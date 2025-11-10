using CongestionTaxCalculator.Domain.TaxCalculation.Entities;

namespace CongestionTaxCalculator.Domain.VehicleManagement.Services.Interfaces
{
	public interface IVehicleTaxExemptionService
	{
		bool IsTaxExempt(Vehicle vehicle, City city);
	}
}

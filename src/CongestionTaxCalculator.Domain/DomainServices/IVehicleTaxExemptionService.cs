using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Domain.Interfaces.DomainServices
{
	public interface IVehicleTaxExemptionService
	{
		bool IsTaxExempt(Vehicle vehicle, City city);
	}
}

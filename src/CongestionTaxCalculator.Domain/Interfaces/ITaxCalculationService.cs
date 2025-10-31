using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.ValueObjects;

namespace CongestionTaxCalculator.Domain.Interfaces
{
	public interface ITaxCalculationService
	{
		Task<Money> CalculateDailyTax(Vehicle vehicle, City city, List<DateTime> timestamps);
	}
}

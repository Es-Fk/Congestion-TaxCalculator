using CongestionTaxCalculator.Domain.TaxCalculation.Entities;
using CongestionTaxCalculator.Domain.TaxCalculation.ValueObjects;

namespace CongestionTaxCalculator.Domain.CityManagement.Services.Interfaces
{
	public interface ICongestionTaxCalculatorService
	{
		Money CalculateTax(City city, Vehicle vehicle, IEnumerable<DateTime> passages);
	}
}

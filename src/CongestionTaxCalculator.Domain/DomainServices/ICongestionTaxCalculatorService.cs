using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.ValueObjects;

namespace CongestionTaxCalculator.Domain.DomainServices
{
	public interface ICongestionTaxCalculatorService
	{
		Money CalculateTax(City city, Vehicle vehicle, IEnumerable<DateTime> passages);
	}
}

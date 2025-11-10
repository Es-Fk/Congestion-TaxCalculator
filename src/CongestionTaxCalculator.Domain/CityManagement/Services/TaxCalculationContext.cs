using CongestionTaxCalculator.Domain.TaxCalculation.Entities;

namespace CongestionTaxCalculator.Domain.CityManagement.Services
{
	public class TaxCalculationContext
	{
		public City City { get; init; }
		public Vehicle Vehicle { get; init; }
		public DateTime Passage { get; init; }
	}
}

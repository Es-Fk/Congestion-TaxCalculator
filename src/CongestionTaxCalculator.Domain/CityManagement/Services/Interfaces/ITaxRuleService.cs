using CongestionTaxCalculator.Domain.TaxCalculation.Entities;
using CongestionTaxCalculator.Domain.TaxCalculation.ValueObjects;

namespace CongestionTaxCalculator.Domain.CityManagement.Services.Interfaces
{
	public interface ITaxRuleService
	{
		Money GetTaxForTime(DateTime Passage,City city);
	}
}

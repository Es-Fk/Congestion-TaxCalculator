using CongestionTaxCalculator.Domain.TaxCalculation.ValueObjects;

namespace CongestionTaxCalculator.Domain.CityManagement.Services.Interfaces
{
	public interface ITaxRuleHandler
	{
		Money Handle(TaxCalculationContext context);
		void SetNext(ITaxRuleHandler next);
	}
}

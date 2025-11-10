using CongestionTaxCalculator.Domain.CityManagement.Services.Interfaces;
using CongestionTaxCalculator.Domain.TaxCalculation.Entities;
using CongestionTaxCalculator.Domain.TaxCalculation.ValueObjects;

namespace CongestionTaxCalculator.Domain.TaxCalculation.DomainServices
{
	public class TaxRuleService : ITaxRuleService
	{
		public Money GetTaxForTime(DateTime Passage, City city)
		{
			var rule = city.TaxRules
				.FirstOrDefault(x => x.StartTime <= Passage.TimeOfDay && x.EndTime >= Passage.TimeOfDay);
			return rule?.Amount ?? Money.Zero();
		}
	}
}

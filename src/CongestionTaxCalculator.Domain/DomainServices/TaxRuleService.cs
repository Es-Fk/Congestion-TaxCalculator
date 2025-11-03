using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.ValueObjects;

namespace CongestionTaxCalculator.Domain.DomainServices
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

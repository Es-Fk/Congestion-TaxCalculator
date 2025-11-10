using CongestionTaxCalculator.Domain.CityManagement.Services.Interfaces;
using CongestionTaxCalculator.Domain.TaxCalculation.ValueObjects;

namespace CongestionTaxCalculator.Domain.CityManagement.Services
{
	public class BaseTaxRuleHandler : ITaxRuleHandler
	{
		private readonly ITaxRuleService _taxService;

		public BaseTaxRuleHandler(ITaxRuleService taxService)
			=> _taxService = taxService;

		public Money Handle(TaxCalculationContext context)
			=> _taxService.GetTaxForTime(context.Passage, context.City);

		public void SetNext(ITaxRuleHandler next)
		{
			throw new NotImplementedException();
		}
	}

}

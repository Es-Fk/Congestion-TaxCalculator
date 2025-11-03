using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.ValueObjects;

namespace CongestionTaxCalculator.Domain.DomainServices
{
	public interface ITaxRuleService
	{
		Money GetTaxForTime(DateTime Passage,City city);
	}
}

using CongestionTaxCalculator.Domain.Common;
using CongestionTaxCalculator.Domain.ValueObjects;

namespace CongestionTaxCalculator.Domain.Entities
{
	public class TaxRule : AuditableBaseEntity<int>
	{
		public TimeSpan StartTime { get; private set; }
		public TimeSpan EndTime { get; private set; }
		public Money Amount { get; private set; }
		public TaxRule() { }
		public TaxRule(TimeSpan startTime, TimeSpan endTime, Money amount)
		{
			StartTime = startTime;
			EndTime = endTime;
			Amount = amount;
		}
		public bool IsApplicable(DateTime timestamp)
		{
			var timeOfDay = timestamp.TimeOfDay;
			return timeOfDay >= StartTime && timeOfDay <= EndTime;
		}
	}
}

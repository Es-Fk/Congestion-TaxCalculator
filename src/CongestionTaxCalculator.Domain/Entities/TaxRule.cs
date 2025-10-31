using CongestionTaxCalculator.Domain.Common;
using CongestionTaxCalculator.Domain.ValueObjects;

namespace CongestionTaxCalculator.Domain.Entities
{
	public class TaxRule : BaseEntity<int>
	{
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public Money Amount { get; set; }

		public bool IsApplicable(DateTime timestamp)
		{
			var timeOfDay = timestamp.TimeOfDay;
			return timeOfDay >= StartTime && timeOfDay <= EndTime;
		}
	}
}

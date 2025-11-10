using CongestionTaxCalculator.Domain.Calendar.Services.Interfaces;
using CongestionTaxCalculator.Domain.CityManagement.Services;
using CongestionTaxCalculator.Domain.CityManagement.Services.Interfaces;
using CongestionTaxCalculator.Domain.TaxCalculation.ValueObjects;

namespace CongestionTaxCalculator.Domain.Calendar.Services
{
	public class HolidayExemptionHandler : ITaxRuleHandler
	{
		private readonly IHolidayService _holidayService;
		private ITaxRuleHandler _next;

		public HolidayExemptionHandler(IHolidayService holidayService)
		{
			_holidayService = holidayService;
		}

		public Money Handle(TaxCalculationContext context)
		{
			var city = context.City;
			var date = context.Passage;

			if (city.IsWeekendTaxExempt && date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
				return Money.Zero();

			if (city.IsHolidayTaxExempt && _holidayService.IsHoliday(date, city))
				return Money.Zero();

			if (city.IsDayBeforeHolidayTaxExempt && _holidayService.IsDayBeforeHoliday(date, city))
				return Money.Zero();

			if (city.IsJulyTaxExempt && date.Month == 7)
				return Money.Zero();

			return _next?.Handle(context) ?? Money.Zero();
		}

		public void SetNext(ITaxRuleHandler next) => _next = next;
	}


}

using CongestionTaxCalculator.Domain.Calendar.Services.Interfaces;
using CongestionTaxCalculator.Domain.TaxCalculation.Entities;

namespace CongestionTaxCalculator.Domain.TaxCalculation.DomainServices
{
	public class HolidayService : IHolidayService
	{
		public bool IsHoliday(DateTime date, City city)
		{
			return city.Holidays.Any(h => h.Date == DateOnly.FromDateTime(date.Date));
		}

		public bool IsDayBeforeHoliday(DateTime date, City city)
		{
			return city.Holidays.Any(h => h.Date.AddDays(-1) == DateOnly.FromDateTime(date.Date));
		}
	}
}

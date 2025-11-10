using CongestionTaxCalculator.Domain.TaxCalculation.Entities;

namespace CongestionTaxCalculator.Domain.Calendar.Services.Interfaces
{
	public interface IHolidayService
	{
		bool IsHoliday(DateTime date, City city);
		bool IsDayBeforeHoliday(DateTime date, City city);
	}
}

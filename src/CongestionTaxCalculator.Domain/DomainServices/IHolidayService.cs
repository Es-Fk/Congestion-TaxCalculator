using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Domain.DomainServices
{
	public interface IHolidayService
	{
		bool IsHoliday(DateTime date, City city);
		bool IsDayBeforeHoliday(DateTime date, City city);
	}
}

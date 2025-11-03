using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Interfaces.DomainServices;
using CongestionTaxCalculator.Domain.ValueObjects;

namespace CongestionTaxCalculator.Domain.DomainServices
{
	public class CongestionTaxCalculatorService : ICongestionTaxCalculatorService
	{
		private readonly IVehicleTaxExemptionService _vehicleExemptionService;
		private readonly IHolidayService _holidayService;
		private readonly ITaxRuleService _taxRuleService;
		public CongestionTaxCalculatorService()
		{
			_vehicleExemptionService = new VehicleTaxExemptionService();
			_holidayService = new HolidayService();
			_taxRuleService = new TaxRuleService();
		}

		public Money CalculateTax(City city, Vehicle vehicle, IEnumerable<DateTime> passages)
		{
			ArgumentNullException.ThrowIfNull(city);
			ArgumentNullException.ThrowIfNull(vehicle);
			ArgumentNullException.ThrowIfNull(passages);

			if (_vehicleExemptionService.IsTaxExempt(vehicle, city))
				return Money.Zero();

			var groupedByDay = passages.OrderBy(p => p).GroupBy(p => p.Date);
			Money totalTax = Money.Zero();

			foreach (var day in groupedByDay)
			{
				var dayPassages = day.OrderBy(p => p).ToList();
				var dayTax = CalculateDailyTax(dayPassages, city);

				// Apply daily cap
				totalTax += Money.Min(dayTax, city.MaximumTaxPerDay);
			}

			return totalTax;
		}

		private Money CalculateDailyTax(List<DateTime> passages, City city)
		{
			if (passages == null || !passages.Any())
				return Money.Zero();

			Money dayTotal = Money.Zero();
			DateTime? windowStart = null;
			Money maxTaxInWindow = Money.Zero();

			foreach (var passage in passages)
			{
				if (IsTaxFreeDate(passage, city))
					continue;

				var tax = _taxRuleService.GetTaxForTime(passage, city);

				if (windowStart == null)
				{
					windowStart = passage;
					maxTaxInWindow = tax;
					continue;
				}

				var diffMinutes = (passage - windowStart.Value).TotalMinutes;

				if (diffMinutes <= city.SingleChargeDurationMinutes)
				{
					maxTaxInWindow = Money.Max(maxTaxInWindow, tax);
				}
				else
				{
					dayTotal += maxTaxInWindow;
					windowStart = passage;
					maxTaxInWindow = tax;
				}
			}

			dayTotal += maxTaxInWindow;

			return dayTotal;
		}

		private bool IsTaxFreeDate(DateTime date, City city)
		{
			if (city.IsWeekendTaxExempt && date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
				return true;

			if (city.IsHolidayTaxExempt && _holidayService.IsHoliday(date, city))
				return true;

			if (city.IsDayBeforeHolidayTaxExempt && _holidayService.IsDayBeforeHoliday(date, city))
				return true;

			if (city.IsJulyTaxExempt && date.Month == 7)
				return true;

			return false;
		}

	}
}

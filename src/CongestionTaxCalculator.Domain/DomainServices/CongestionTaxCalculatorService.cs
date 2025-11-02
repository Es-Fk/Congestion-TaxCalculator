using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Interfaces.DomainServices;
using CongestionTaxCalculator.Domain.ValueObjects;

namespace CongestionTaxCalculator.Domain.DomainServices
{
	public class CongestionTaxCalculatorService : ICongestionTaxCalculatorService
	{
		private readonly IVehicleTaxExemptionService _vehicleExemptionService;
		private readonly IHolidayService _holidayService;

		public CongestionTaxCalculatorService(
			IVehicleTaxExemptionService vehicleExemptionService,
			IHolidayService holidayService)
		{
			_vehicleExemptionService = vehicleExemptionService;
			_holidayService = holidayService;
		}

		public Money CalculateTax(City city, Vehicle vehicle, IEnumerable<DateTime> passages)
		{
			if (city == null) throw new ArgumentNullException(nameof(city));
			if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
			if (passages == null) throw new ArgumentNullException(nameof(passages));

			if (_vehicleExemptionService.IsTaxExempt(vehicle, city))
				return Money.Zero();

			var total = Money.Zero();

			
			var days = passages.OrderBy(p => p).GroupBy(p => p.Date);
			foreach (var dayGroup in days)
			{
				var dayPassages = dayGroup.OrderBy(p => p).ToList();
				var dayTotal = Money.Zero();
				DateTime? intervalStart = null;
				var maxTaxInWindow = Money.Zero();
				// TODO: make this loop as function
				foreach (var time in dayPassages)
				{
					if (IsTaxFreeDate(time, city))
						continue;

					var tax = GetTollFee(time, city);

					// Initialize interval on first taxable passage
					if (intervalStart == null)
					{
						intervalStart = time;
						maxTaxInWindow = tax;
						continue;
					}

					var minutes = (time - intervalStart.Value).TotalMinutes;
					if (minutes <= city.SingleChargeDurationMinutes)
					{
						// Within single charge window: take the highest fee
						if (tax.CompareTo(maxTaxInWindow) > 0)
							maxTaxInWindow = tax;
					}
					else
					{
						// Outside window: add the recorded max and start a new window
						dayTotal += maxTaxInWindow;
						intervalStart = time;
						maxTaxInWindow = tax;
					}
				}

				// Add remaining window's max fee (if any)
				dayTotal += maxTaxInWindow;

				// Apply daily maximum cap
				if (dayTotal.CompareTo(city.MaximumTaxPerDay) > 0)
					dayTotal = city.MaximumTaxPerDay;

				total += dayTotal;
			}

			return total;
		}

		private bool IsTaxFreeDate(DateTime date, City city)
		{
			if (city == null) throw new ArgumentNullException(nameof(city));

			// Weekend exemption
			if (city.IsWeekendTaxExempt && (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
				return true;

			// Holiday exemptions (use date.Date for consistency)
			var dateOnly = date.Date;
			if (city.IsHolidayTaxExempt && _holidayService.IsHoliday(dateOnly, city))
				return true;

			if (city.IsDayBeforeHolidayTaxExempt && _holidayService.IsDayBeforeHoliday(dateOnly, city))
				return true;

			// July exemption
			if (city.IsJulyTaxExempt && date.Month == 7)
				return true;

			return false;
		}

		private Money GetTollFee(DateTime time, City city)
		{
			if (city == null) throw new ArgumentNullException(nameof(city));

			var rule = city.TaxRules?.FirstOrDefault(r => r.IsApplicable(time))
			           ?? city.TaxRules?.FirstOrDefault(r => time.TimeOfDay >= r.StartTime && time.TimeOfDay <= r.EndTime);

			return rule?.Amount ?? Money.Zero();
		}
	}
}

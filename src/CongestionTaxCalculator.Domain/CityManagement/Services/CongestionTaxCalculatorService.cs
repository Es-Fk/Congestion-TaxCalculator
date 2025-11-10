using CongestionTaxCalculator.Domain.Calendar.Services;
using CongestionTaxCalculator.Domain.Calendar.Services.Interfaces;
using CongestionTaxCalculator.Domain.CityManagement.Services;
using CongestionTaxCalculator.Domain.CityManagement.Services.Interfaces;
using CongestionTaxCalculator.Domain.TaxCalculation.Entities;
using CongestionTaxCalculator.Domain.TaxCalculation.ValueObjects;
using CongestionTaxCalculator.Domain.VehicleManagement.Services;
using CongestionTaxCalculator.Domain.VehicleManagement.Services.Interfaces;

namespace CongestionTaxCalculator.Domain.TaxCalculation.DomainServices
{
	public class CongestionTaxCalculatorService : ICongestionTaxCalculatorService
	{
		private readonly ITaxRuleHandler _handlerChain;
		private readonly IVehicleTaxExemptionService _vehicleExemptionService;
		private readonly IHolidayService _holidayService;
		private readonly ITaxRuleService _taxRuleService;

		public CongestionTaxCalculatorService()
		{
			_vehicleExemptionService = new VehicleTaxExemptionService();
			_holidayService = new HolidayService();
			_taxRuleService = new TaxRuleService();

			var vehicleHandler = new VehicleExemptionHandler(_vehicleExemptionService);
			var holidayHandler = new HolidayExemptionHandler(_holidayService);
			var baseHandler = new BaseTaxRuleHandler(_taxRuleService);

			vehicleHandler.SetNext(holidayHandler);
			holidayHandler.SetNext(baseHandler);

			_handlerChain = vehicleHandler;
		}

		public Money CalculateTax(City city, Vehicle vehicle, IEnumerable<DateTime> passages)
		{
			ArgumentNullException.ThrowIfNull(city);
			ArgumentNullException.ThrowIfNull(vehicle);
			ArgumentNullException.ThrowIfNull(passages);

			return AggregateDailyTaxes(passages, city, vehicle);
		}

		private Money AggregateDailyTaxes(IEnumerable<DateTime> passages, City city, Vehicle vehicle)
		{
			var groupedByDay = passages.OrderBy(p => p).GroupBy(p => p.Date);
			Money totalTax = Money.Zero();

			foreach (var day in groupedByDay)
			{
				var dayPassages = day.OrderBy(p => p).ToList();
				var dayTax = CalculateDailyTax(dayPassages, city, vehicle);

				totalTax += Money.Min(dayTax, city.MaximumTaxPerDay);
			}

			return totalTax;
		}

		private Money CalculateDailyTax(List<DateTime> passages, City city, Vehicle vehicle)
		{
			if (passages == null || !passages.Any())
				return Money.Zero();

			Money dayTotal = Money.Zero();
			DateTime? windowStart = null;
			Money maxTaxInWindow = Money.Zero();

			foreach (var passage in passages)
			{
				var context = new TaxCalculationContext
				{
					City = city,
					Vehicle = vehicle,
					Passage = passage
				};

				var tax = _handlerChain.Handle(context);

				if (tax == Money.Zero())
					continue;

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
	}

}

using CongestionTaxCalculator.Domain.Common;
using CongestionTaxCalculator.Domain.ValueObjects;

namespace CongestionTaxCalculator.Domain.Entities
{
	//Aggregate Root
	public class City : AuditableBaseEntity<int>
	{
		public City() { }
		internal City(
			string name,
			Money maximumTaxPerDay,
			int singleChargeDurationMinutes,
			bool isHolidayTaxExempt,
			bool isDayBeforeHolidayTaxExempt,
			bool isWeekendTaxExempt,
			bool isJulyTaxExempt)
		{
			Name = name;
			MaximumTaxPerDay = maximumTaxPerDay;
			SingleChargeDurationMinutes = singleChargeDurationMinutes;
			IsHolidayTaxExempt = isHolidayTaxExempt;
			IsDayBeforeHolidayTaxExempt = isDayBeforeHolidayTaxExempt;
			IsWeekendTaxExempt = isWeekendTaxExempt;
			IsJulyTaxExempt = isJulyTaxExempt;
		}
		public string Name { get; private set; }
		public Money MaximumTaxPerDay { get; private set; }
		public int SingleChargeDurationMinutes { get; private set; }
		public bool IsHolidayTaxExempt { get; private set; }
		public bool IsDayBeforeHolidayTaxExempt { get; private set; }
		public bool IsWeekendTaxExempt { get; private set; }
		public bool IsJulyTaxExempt { get; private set; }

		private readonly List<TaxRule> _taxRules = new();
		public IReadOnlyCollection<TaxRule> TaxRules => _taxRules.AsReadOnly();

		private readonly List<Holiday> _holidays = new();
		public IReadOnlyCollection<Holiday> Holidays => _holidays.AsReadOnly();

		private readonly List<TaxExemptVehicle> _taxExemptVehicles = new();
		public IReadOnlyCollection<TaxExemptVehicle> TaxExemptVehicles => _taxExemptVehicles.AsReadOnly();

		public void AddTaxRule(TaxRule rule) => _taxRules.Add(rule);

		public void AddHoliday(Holiday holiday) => _holidays.Add(holiday);

		public void AddExemptVehicle(TaxExemptVehicle vehicle) => _taxExemptVehicles.Add(vehicle);

		public static City Create(
			string name,
			Money maximumTaxPerDay,
			int singleChargeDurationMinutes,
			bool isHolidayTaxExempt = false,
			bool isDayBeforeHolidayTaxExempt = false,
			bool isWeekendTaxExempt = false,
			bool isJulyTaxExempt = false)
		{
			return new City(
				name,
				maximumTaxPerDay,
				singleChargeDurationMinutes,
				isHolidayTaxExempt,
				isDayBeforeHolidayTaxExempt,
				isWeekendTaxExempt,
				isJulyTaxExempt
			);
		}
	}
}

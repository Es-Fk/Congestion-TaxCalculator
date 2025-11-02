using CongestionTaxCalculator.Domain.Common;
using CongestionTaxCalculator.Domain.ValueObjects;

namespace CongestionTaxCalculator.Domain.Entities
{
	//Aggregate Root
	public class City : AuditableBaseEntity<int>
	{
		public string Name { get; private set; } = default!;
		public Money MaximumTaxPerDay { get; private set; } = default!;
		public uint SingleChargeDurationMinutes { get; private set; } = default!;
		public bool IsHolidayTaxExempt { get; private set; } = default!;
		public bool IsDayBeforeHolidayTaxExempt { get; private set; } = default!;
		public bool IsWeekendTaxExempt { get; private set; } = default!;
		public bool IsJulyTaxExempt { get; private set; } = default!;

		private readonly List<TaxRule> _taxRules = new();
		public IReadOnlyCollection<TaxRule> TaxRules => _taxRules.AsReadOnly();

		private readonly List<Holiday> _holidays = new();
		public IReadOnlyCollection<Holiday> Holidays => _holidays.AsReadOnly();

		private readonly List<TaxExemptVehicle> _taxExemptVehicles = new();
		public IReadOnlyCollection<TaxExemptVehicle> TaxExemptVehicles => _taxExemptVehicles.AsReadOnly();

		public void AddTaxRule(TaxRule rule) => _taxRules.Add(rule);

		public void AddHoliday(Holiday holiday) => _holidays.Add(holiday);

		public void AddExemptVehicle(TaxExemptVehicle vehicle) => _taxExemptVehicles.Add(vehicle);
	}
}

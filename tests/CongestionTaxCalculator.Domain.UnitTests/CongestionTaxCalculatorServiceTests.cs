using CongestionTaxCalculator.Domain.CityManagement.Entities;
using CongestionTaxCalculator.Domain.TaxCalculation.DomainServices;
using CongestionTaxCalculator.Domain.TaxCalculation.Entities;
using CongestionTaxCalculator.Domain.TaxCalculation.ValueObjects;
using CongestionTaxCalculator.Domain.VehicleManagement.Entities.Enums;

namespace CongestionTaxCalculator.Domain.UnitTests
{
	public class CongestionTaxCalculatorServiceTests
	{
		private City BuildGothenburg()
		{
			var city = City.Create("Gothenburg", new Money(20m), 60, isHolidayTaxExempt: true, isDayBeforeHolidayTaxExempt: true, isWeekendTaxExempt: true, isJulyTaxExempt: true);

			city.AddTaxRule(new TaxRule(new TimeSpan(6, 0, 0), new TimeSpan(6, 30, 0), new Money(8m)));
			city.AddTaxRule(new TaxRule(new TimeSpan(6, 30, 0), new TimeSpan(7, 0, 0), new Money(13m)));
			city.AddTaxRule(new TaxRule(new TimeSpan(7, 0, 0), new TimeSpan(8, 0, 0), new Money(18m)));

			// add an exempt vehicle type (Motorcycle)
			city.AddExemptVehicle(new TaxExemptVehicle(new Vehicle(Guid.NewGuid(), VehicleType.Motorcycle, "EX1")));

			// add a holiday
			city.AddHoliday(new Holiday(new DateOnly(2025, 12, 25), "Xmas"));

			return city;
		}

		[Fact]
		public void CalculateTax_SinglePassage_ReturnsRuleAmount()
		{
			var city = BuildGothenburg();
			var vehicle = new Vehicle(Guid.NewGuid(), VehicleType.Car, "C1");
			var svc = new CongestionTaxCalculatorService();

			var passages = new[] { new DateTime(2025, 1, 1, 6, 15, 0) };
			var tax = svc.CalculateTax(city, vehicle, passages);

			Assert.Equal(new Money(8m), tax);
		}

		[Fact]
		public void CalculateTax_MultipleWithinSingleCharge_UsesMaxInWindow()
		{
			var city = BuildGothenburg();
			var vehicle = new Vehicle(Guid.NewGuid(), VehicleType.Car, "C2");
			var svc = new CongestionTaxCalculatorService();

			// 6:20 -> rule 8, 6:45 -> rule 13, within 60 minutes => max 13
			var passages = new[]
			{
				new DateTime(2025, 1, 1, 6, 20, 0),
				new DateTime(2025, 1, 1, 6, 45, 0)
			};

			var tax = svc.CalculateTax(city, vehicle, passages);
			Assert.Equal(new Money(13m), tax);
		}

		[Fact]
		public void CalculateTax_MultipleSeparated_CountsBoth_ApplyDailyCap()
		{
			var city = BuildGothenburg();
			// set daily cap low so cap is hit
			var cityWithLowCap = City.Create("LowCap", new Money(20m), 60);
			cityWithLowCap.AddTaxRule(new TaxRule(new TimeSpan(6, 0, 0), new TimeSpan(6, 30, 0), new Money(8m)));
			cityWithLowCap.AddTaxRule(new TaxRule(new TimeSpan(7, 0, 0), new TimeSpan(8, 0, 0), new Money(18m)));

			var vehicle = new Vehicle(Guid.NewGuid(), VehicleType.Car, "C3");
			var svc = new CongestionTaxCalculatorService();

			// 6:00 -> 8, 7:30 -> 18, diff > 60 => both counted => 26 but capped to 20
			var passages = new[]
			{
				new DateTime(2025, 1, 1, 6, 0, 0),
				new DateTime(2025, 1, 1, 7, 30, 0)
			};

			var tax = svc.CalculateTax(cityWithLowCap, vehicle, passages);
			Assert.Equal(new Money(20m), tax);
		}

		[Fact]
		public void CalculateTax_ExemptVehicle_ReturnsZero()
		{
			var city = BuildGothenburg();
			// motorcycle is configured as exempt in BuildGothenburg
			var exemptVehicle = new Vehicle(Guid.NewGuid(), VehicleType.Motorcycle, "MOTO");
			var svc = new CongestionTaxCalculatorService();

			var passages = new[] { new DateTime(2025, 1, 1, 6, 15, 0) };
			var tax = svc.CalculateTax(city, exemptVehicle, passages);
			Assert.Equal(Money.Zero(), tax);
		}

		[Fact]
		public void CalculateTax_HolidayAndJulyAndWeekend_ExemptWhenConfigured()
		{
			var city = BuildGothenburg();
			var vehicle = new Vehicle(Guid.NewGuid(), VehicleType.Car, "C4");
			var svc = new CongestionTaxCalculatorService();

			// holiday (configured in BuildGothenburg)
			var holidayPass = new[] { new DateTime(2025, 12, 25, 7, 0, 0) };
			Assert.Equal(Money.Zero(), svc.CalculateTax(city, vehicle, holidayPass));

			// july month (IsJulyTaxExempt true)
			var julyPass = new[] { new DateTime(2025, 7, 10, 8, 30, 0) };
			Assert.Equal(Money.Zero(), svc.CalculateTax(city, vehicle, julyPass));

			// weekend (Saturday)
			var weekendPass = new[] { new DateTime(2025, 1, 4, 7, 0, 0) }; // 2025-01-04 is Saturday
			Assert.Equal(Money.Zero(), svc.CalculateTax(city, vehicle, weekendPass));
		}
	}
}
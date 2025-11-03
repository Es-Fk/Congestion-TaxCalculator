using CongestionTaxCalculator.Domain.DomainServices;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Entities.Enums;
using CongestionTaxCalculator.Domain.ValueObjects;

namespace CongestionTaxCalculator.Domain.UnitTests
{
	public class TaxRuleAndEntityTests
	{
		[Fact]
		public void TaxRule_IsApplicable_Works()
		{
			var rule = new TaxRule(new TimeSpan(6, 0, 0), new TimeSpan(6, 30, 0), new Money(8m));
			Assert.True(rule.IsApplicable(new DateTime(2025, 1, 1, 6, 0, 0)));
			Assert.True(rule.IsApplicable(new DateTime(2025, 1, 1, 6, 30, 0)));
			Assert.False(rule.IsApplicable(new DateTime(2025, 1, 1, 5, 59, 0)));
			Assert.False(rule.IsApplicable(new DateTime(2025, 1, 1, 6, 31, 0)));
		}

		[Fact]
		public void City_CreateAndAddCollections_Work()
		{
			var city = City.Create("TestCity", new Money(50m), 60, isHolidayTaxExempt: true, isDayBeforeHolidayTaxExempt: true, isWeekendTaxExempt: true, isJulyTaxExempt: true);

			Assert.Equal("TestCity", city.Name);
			Assert.Equal(50m, city.MaximumTaxPerDay.Amount);
			Assert.Equal(60, city.SingleChargeDurationMinutes);

			city.AddTaxRule(new TaxRule(new TimeSpan(6, 0, 0), new TimeSpan(6, 30, 0), new Money(8m)));
			Assert.Single(city.TaxRules);

			city.AddHoliday(new Holiday(new DateOnly(2025, 12, 25), "Xmas"));
			Assert.Single(city.Holidays);

			var v = new Vehicle(Guid.NewGuid(), VehicleType.Motorcycle, "REG1");
			city.AddExemptVehicle(new TaxExemptVehicle(v));
			Assert.Single(city.TaxExemptVehicles);
		}

		[Fact]
		public void TaxRuleService_Returns_CorrectTax()
		{
			var city = City.Create("T", new Money(100m), 60);
			city.AddTaxRule(new TaxRule(new TimeSpan(6, 0, 0), new TimeSpan(6, 30, 0), new Money(8m)));
			city.AddTaxRule(new TaxRule(new TimeSpan(6, 30, 0), new TimeSpan(7, 0, 0), new Money(13m)));
			city.AddTaxRule(new TaxRule(new TimeSpan(7, 0, 0), new TimeSpan(8, 0, 0), new Money(18m)));

			var svc = new TaxRuleService();

			var t1 = svc.GetTaxForTime(new DateTime(2025, 1, 1, 6, 15, 0), city);
			Assert.Equal(new Money(8m), t1);

			var t2 = svc.GetTaxForTime(new DateTime(2025, 1, 1, 6, 40, 0), city);
			Assert.Equal(new Money(13m), t2);

			var t3 = svc.GetTaxForTime(new DateTime(2025, 1, 1, 9, 0, 0), city);
			Assert.Equal(Money.Zero(), t3);
		}

		[Fact]
		public void HolidayService_IsHolidayAndDayBefore_Works()
		{
			var city = City.Create("T", new Money(10m), 60);
			city.AddHoliday(new Holiday(new DateOnly(2025, 12, 25), "Xmas"));

			var svc = new HolidayService();
			Assert.True(svc.IsHoliday(new DateTime(2025, 12, 25), city));
			Assert.False(svc.IsHoliday(new DateTime(2025, 12, 24), city));

			Assert.True(svc.IsDayBeforeHoliday(new DateTime(2025, 12, 24), city));
			Assert.False(svc.IsDayBeforeHoliday(new DateTime(2025, 12, 23), city));
		}

		[Fact]
		public void VehicleTaxExemptionService_Works()
		{
			var city = City.Create("T", new Money(10m), 60);
			var exemptVehicle = new Vehicle(Guid.NewGuid(), VehicleType.Motorcycle, "M1");
			city.AddExemptVehicle(new TaxExemptVehicle(exemptVehicle));

			var svc = new VehicleTaxExemptionService();
			var v1 = new Vehicle(Guid.NewGuid(), VehicleType.Car, "C1");
			var v2 = new Vehicle(Guid.NewGuid(), VehicleType.Motorcycle, "M2");

			Assert.False(svc.IsTaxExempt(v1, city));
			Assert.True(svc.IsTaxExempt(v2, city)); // matches exempt vehicle type
		}

		[Fact]
		public void TollPassage_SetAppliedTax_Works()
		{
			var passage = new TollPassage(1, Guid.NewGuid(), new DateTime(2025, 1, 1, 7, 0, 0));
			Assert.Null(passage.AppliedTax);

			passage.SetAppliedTax(new Money(13m));
			Assert.Equal(new Money(13m), passage.AppliedTax);
		}
	}
}
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Entities.Enums;
using CongestionTaxCalculator.Domain.ValueObjects;
using CongestionTaxCalculator.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace CongestionTaxCalculator.IntegrationTests
{
	public class CustomWebApplicationFactory : WebApplicationFactory<Program>
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				var descriptor = services.SingleOrDefault(
					d => d.ServiceType == typeof(DbContextOptions<CongestionTaxDbContext>));
				if (descriptor != null)
					services.Remove(descriptor);
				services.AddDbContext<CongestionTaxDbContext>(options =>
				{
					options.UseInMemoryDatabase("CongestionTaxInMemoryDb");
					options.EnableSensitiveDataLogging();
					options.LogTo(Console.WriteLine, LogLevel.Information);
				});
				var sp = services.BuildServiceProvider();
				using var scope = sp.CreateScope();
				var db = scope.ServiceProvider.GetRequiredService<CongestionTaxDbContext>();

				db.Database.EnsureCreated();
				SeedTestData(db);
			});

		}

		private static void SeedTestData(CongestionTaxDbContext db)
		{
			var city = City.Create(
				name: "Gothenburg",
				maximumTaxPerDay: new Money(60m, "SEK"),
				singleChargeDurationMinutes: 60,
				isHolidayTaxExempt: true,
				isDayBeforeHolidayTaxExempt: true,
				isWeekendTaxExempt: true,
				isJulyTaxExempt: true
			);
			city.SetRowVersion(Guid.NewGuid().ToByteArray());

			var holiday1 = new Holiday(new DateOnly(2025, 1, 1));
			holiday1.SetRowVersion(Guid.NewGuid().ToByteArray());
			city.AddHoliday(holiday1);

			var holiday2 = new Holiday(new DateOnly(2025, 12, 25));
			holiday2.SetRowVersion(Guid.NewGuid().ToByteArray());
			city.AddHoliday(holiday2);

			{
				var v = new Vehicle(Guid.NewGuid(), VehicleType.Military, "C1");
				v.SetRowVersion(Guid.NewGuid().ToByteArray());
				var tev = new TaxExemptVehicle(v);
				tev.SetRowVersion(Guid.NewGuid().ToByteArray());
				city.AddExemptVehicle(tev);
			}
			{
				var v = new Vehicle(Guid.NewGuid(), VehicleType.Motorcycle, "C2");
				v.SetRowVersion(Guid.NewGuid().ToByteArray());
				var tev = new TaxExemptVehicle(v);
				tev.SetRowVersion(Guid.NewGuid().ToByteArray());
				city.AddExemptVehicle(tev);
			}
			{
				var v = new Vehicle(Guid.NewGuid(), VehicleType.Emergency, "C3");
				v.SetRowVersion(Guid.NewGuid().ToByteArray());
				var tev = new TaxExemptVehicle(v);
				tev.SetRowVersion(Guid.NewGuid().ToByteArray());
				city.AddExemptVehicle(tev);
			}
			{
				var v = new Vehicle(Guid.NewGuid(), VehicleType.Diplomat, "C4");
				v.SetRowVersion(Guid.NewGuid().ToByteArray());
				var tev = new TaxExemptVehicle(v);
				tev.SetRowVersion(Guid.NewGuid().ToByteArray());
				city.AddExemptVehicle(tev);
			}
			{
				var v = new Vehicle(Guid.NewGuid(), VehicleType.Bus, "C5");
				v.SetRowVersion(Guid.NewGuid().ToByteArray());
				var tev = new TaxExemptVehicle(v);
				tev.SetRowVersion(Guid.NewGuid().ToByteArray());
				city.AddExemptVehicle(tev);
			}
			{
				var v = new Vehicle(Guid.NewGuid(), VehicleType.Foreign, "C6");
				v.SetRowVersion(Guid.NewGuid().ToByteArray());
				var tev = new TaxExemptVehicle(v);
				tev.SetRowVersion(Guid.NewGuid().ToByteArray());
				city.AddExemptVehicle(tev);
			}

			var tr1 = new TaxRule(
				startTime: new TimeSpan(6, 0, 0),
				endTime: new TimeSpan(6, 30, 0),
				amount: new Money(8m, "SEK")
			);
			tr1.SetRowVersion(Guid.NewGuid().ToByteArray());
			city.AddTaxRule(tr1);

			var tr2 = new TaxRule(
				startTime: new TimeSpan(6, 30, 0),
				endTime: new TimeSpan(7, 0, 0),
				amount: new Money(13m, "SEK")
			);
			tr2.SetRowVersion(Guid.NewGuid().ToByteArray());
			city.AddTaxRule(tr2);

			var tr3 = new TaxRule(
				startTime: new TimeSpan(7, 0, 0),
				endTime: new TimeSpan(8, 0, 0),
				amount: new Money(18m, "SEK")
			);
			tr3.SetRowVersion(Guid.NewGuid().ToByteArray());
			city.AddTaxRule(tr3);

			db.Cities.Add(city);
			db.SaveChanges();
		}
	}
}

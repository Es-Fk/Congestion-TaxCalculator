using CongestionTaxCalculator.Application.Common.Exceptions;
using CongestionTaxCalculator.Application.Features.CalculateCongestionTax.Commands;
using CongestionTaxCalculator.Domain.DomainServices;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Entities.Enums;
using CongestionTaxCalculator.Domain.Interfaces.Repositories;
using CongestionTaxCalculator.Domain.ValueObjects;
using Moq;

namespace CongestionTaxCalculator.Aplication.UnitTests
{
	public class CalculateCongestionTaxCommandHandlerTests
	{
		[Fact]
		public async Task Handle_CityNotFound_ThrowsNotFoundException()
		{
			// Arrange
			var cityRepo = new Mock<ICityRepository>();
			cityRepo.Setup(r => r.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((City?)null);

			var vehicleRepo = new Mock<IVehicleRepository>();
			var taxCalculator = new Mock<ICongestionTaxCalculatorService>();

			var handler = new CalculateCongestionTaxCommandHandler(cityRepo.Object, vehicleRepo.Object, taxCalculator.Object);

			var command = new CalculateCongestionTaxCommand("NonExistingCity", VehicleType.Car, new List<DateTime> { DateTime.Now });

			// Act & Assert
			await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
		}

		[Fact]
		public async Task Handle_VehicleNotFound_ThrowsNotFoundException()
		{
			// Arrange
			var city = City.Create("Gothenburg", new Money(60m), 60, isHolidayTaxExempt: true, isDayBeforeHolidayTaxExempt: true, isWeekendTaxExempt: true, isJulyTaxExempt: true);

			var cityRepo = new Mock<ICityRepository>();
			cityRepo.Setup(r => r.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(city);

			var vehicleRepo = new Mock<IVehicleRepository>();
			vehicleRepo.Setup(r => r.GetByTypeAsync(It.IsAny<VehicleType>(), It.IsAny<CancellationToken>())).ReturnsAsync((Vehicle?)null);

			var taxCalculator = new Mock<ICongestionTaxCalculatorService>();

			var handler = new CalculateCongestionTaxCommandHandler(cityRepo.Object, vehicleRepo.Object, taxCalculator.Object);

			var command = new CalculateCongestionTaxCommand("Gothenburg", VehicleType.Motorcycle, new List<DateTime> { DateTime.Now });

			// Act & Assert
			await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
		}

		[Fact]
		public async Task Handle_ValidRequest_ReturnsCalculatedMoney()
		{
			// Arrange
			var city = City.Create("Gothenburg", new Money(60m), 60, isHolidayTaxExempt: true, isDayBeforeHolidayTaxExempt: true, isWeekendTaxExempt: true, isJulyTaxExempt: true);
			var vehicle = new Vehicle(Guid.NewGuid(), VehicleType.Car, "ABC123");

			var cityRepo = new Mock<ICityRepository>();
			cityRepo.Setup(r => r.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(city);

			var vehicleRepo = new Mock<IVehicleRepository>();
			vehicleRepo.Setup(r => r.GetByTypeAsync(It.IsAny<VehicleType>(), It.IsAny<CancellationToken>())).ReturnsAsync(vehicle);

			var expected = new Money(42m);
			var taxCalculator = new Mock<ICongestionTaxCalculatorService>();
			taxCalculator.Setup(t => t.CalculateTax(city, vehicle, It.IsAny<IEnumerable<DateTime>>())).Returns(expected);

			var handler = new CalculateCongestionTaxCommandHandler(cityRepo.Object, vehicleRepo.Object, taxCalculator.Object);

			var passageTimes = new List<DateTime> { DateTime.Now.AddMinutes(-120), DateTime.Now.AddMinutes(-60) };
			var command = new CalculateCongestionTaxCommand("Gothenburg", VehicleType.Car, passageTimes);

			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.Equal(expected, result);
		}
	}
}

using CongestionTaxCalculator.Application.Features.CalculateCongestionTax.Commands;
using CongestionTaxCalculator.Domain.VehicleManagement.Entities.Enums;

namespace CongestionTaxCalculator.Aplication.UnitTests
{
	public class CalculateCongestionTaxCommandValidatorTests
	{
		[Fact]
		public void Validator_InvalidModel_ReturnsFailures()
		{
			// Arrange
			var validator = new CalculateCongestionTaxCommandValidator();

			// Empty city name
			var cmd1 = new CalculateCongestionTaxCommand("", VehicleType.Car, new List<DateTime> { DateTime.Now.AddMinutes(-10) });
			var res1 = validator.Validate(cmd1);
			Assert.False(res1.IsValid);
			Assert.Contains(res1.Errors, e => e.PropertyName == "CityName");

			// Passage time in future
			var cmd2 = new CalculateCongestionTaxCommand("Gothenburg", VehicleType.Car, new List<DateTime> { DateTime.Now.AddMinutes(10) });
			var res2 = validator.Validate(cmd2);
			Assert.False(res2.IsValid);
			Assert.Contains(res2.Errors, e => e.PropertyName == "PassageTimes[0]" || e.PropertyName == "PassageTimes");
		}

		[Fact]
		public void Validator_ValidModel_NoFailures()
		{
			// Arrange
			var validator = new CalculateCongestionTaxCommandValidator();
			var cmd = new CalculateCongestionTaxCommand("Gothenburg", VehicleType.Car, new List<DateTime> { DateTime.Now.AddMinutes(-10) });

			// Act
			var result = validator.Validate(cmd);

			// Assert
			Assert.True(result.IsValid);
		}
	}

}

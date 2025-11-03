using FluentValidation;

namespace CongestionTaxCalculator.Application.Features.CalculateCongestionTax.Commands
{
	public class CalculateCongestionTaxCommandValidator : AbstractValidator<CalculateCongestionTaxCommand>
	{
		public CalculateCongestionTaxCommandValidator()
		{
			RuleFor(x => x.CityName)
				.NotEmpty().WithMessage("City name is required.");

			RuleFor(x => x.VehicleType)
				.IsInEnum().WithMessage("Invalid vehicle type.");

			RuleFor(x => x.PassageTimes)
				.NotEmpty().WithMessage("At least one passage time is required.");

			RuleForEach(x => x.PassageTimes)
				.LessThanOrEqualTo(DateTime.Now)
				.WithMessage("Passage time cannot be in the future.");
		}
	}
}

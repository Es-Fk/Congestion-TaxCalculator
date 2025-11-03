using CongestionTaxCalculator.Domain.Entities.Enums;
using CongestionTaxCalculator.Domain.ValueObjects;
using MediatR;

namespace CongestionTaxCalculator.Application.Features.CalculateCongestionTax.Commands
{
	public record CalculateCongestionTaxCommand(
        string CityName,
        VehicleType VehicleType,
        List<DateTime> PassageTimes
    ) : IRequest<Money>;
}

using CongestionTaxCalculator.Application.Dtos;
using MediatR;

namespace CongestionTaxCalculator.Application.Features.CalculateCongestionTax.Commands
{
	public record CalculateCongestionTaxCommand(
        int CityId,
        Guid VehicleId,
        List<DateTime> PassageTimes
    ) : IRequest<CalculateCongestionTaxResult>;
}

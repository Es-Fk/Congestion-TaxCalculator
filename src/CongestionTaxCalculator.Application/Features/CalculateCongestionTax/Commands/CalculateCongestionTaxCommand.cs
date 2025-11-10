using CongestionTaxCalculator.Domain.TaxCalculation.ValueObjects;
using CongestionTaxCalculator.Domain.VehicleManagement.Entities.Enums;
using MediatR;

namespace CongestionTaxCalculator.Application.Features.CalculateCongestionTax.Commands
{
	public record CalculateCongestionTaxCommand(
        string CityName,
        VehicleType VehicleType,
        List<DateTime> PassageTimes
    ) : IRequest<Money>;
}

using CongestionTaxCalculator.Application.Dtos;
using CongestionTaxCalculator.Domain.DomainServices;
using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Interfaces.Repositories;
using CongestionTaxCalculator.Infrastructure.Exceptions;
using MediatR;

namespace CongestionTaxCalculator.Application.Features.CalculateCongestionTax.Commands
{
	public class CalculateCongestionTaxCommandHandler: IRequestHandler<CalculateCongestionTaxCommand, CalculateCongestionTaxResult>
	{
		private readonly ICityRepository _cityRepository;
		private readonly IVehicleRepository _vehicleRepository;
		private readonly ICongestionTaxCalculatorService _taxCalculator;

		public CalculateCongestionTaxCommandHandler(
			ICityRepository cityRepository,
			IVehicleRepository vehicleRepository,
			ICongestionTaxCalculatorService taxCalculator)
		{
			_cityRepository = cityRepository;
			_vehicleRepository = vehicleRepository;
			_taxCalculator = taxCalculator;
		}

		public async Task<CalculateCongestionTaxResult> Handle(
			CalculateCongestionTaxCommand request,
			CancellationToken cancellationToken)
		{
			var city = await _cityRepository.GetByIdAsync(request.CityId, cancellationToken);
			if (city == null)
				throw new NotFoundException(nameof(City), request.CityId);

			var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);
			if (vehicle == null)
				throw new NotFoundException(nameof(Vehicle), request.VehicleId);

			var tax = _taxCalculator.CalculateTax(city, vehicle, request.PassageTimes);

			return new CalculateCongestionTaxResult(
				city.Name,
				vehicle.RegistrationNumber,
				tax.Amount,
				request.PassageTimes.Count
			);
		}
	}

}

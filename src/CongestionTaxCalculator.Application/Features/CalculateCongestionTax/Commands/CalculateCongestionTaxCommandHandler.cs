using CongestionTaxCalculator.Application.Common.Exceptions;
using CongestionTaxCalculator.Domain.CityManagement.Services.Interfaces;
using CongestionTaxCalculator.Domain.TaxCalculation.Entities;
using CongestionTaxCalculator.Domain.TaxCalculation.Repositories;
using CongestionTaxCalculator.Domain.TaxCalculation.ValueObjects;
using MediatR;

namespace CongestionTaxCalculator.Application.Features.CalculateCongestionTax.Commands
{
	public class CalculateCongestionTaxCommandHandler : IRequestHandler<CalculateCongestionTaxCommand, Money>
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

		public async Task<Money> Handle(
			CalculateCongestionTaxCommand request,
			CancellationToken cancellationToken)
		{
			var city = await _cityRepository.GetByNameAsync(request.CityName, cancellationToken);
			if (city == null)
				throw new NotFoundException(nameof(City), request.CityName);

			var vehicle = await _vehicleRepository.GetByTypeAsync(request.VehicleType, cancellationToken);
			if (vehicle == null)
				throw new NotFoundException(nameof(Vehicle), request.VehicleType);

			return _taxCalculator.CalculateTax(city, vehicle, request.PassageTimes);

		}
	}

}

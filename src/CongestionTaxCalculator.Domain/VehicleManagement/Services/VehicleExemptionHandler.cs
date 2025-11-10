using CongestionTaxCalculator.Domain.CityManagement.Services;
using CongestionTaxCalculator.Domain.CityManagement.Services.Interfaces;
using CongestionTaxCalculator.Domain.TaxCalculation.ValueObjects;
using CongestionTaxCalculator.Domain.VehicleManagement.Services.Interfaces;

namespace CongestionTaxCalculator.Domain.VehicleManagement.Services
{
	public class VehicleExemptionHandler : ITaxRuleHandler
	{
		private readonly IVehicleTaxExemptionService _service;
		private ITaxRuleHandler _next;

		public VehicleExemptionHandler(IVehicleTaxExemptionService service)
			=> _service = service;

		public Money Handle(TaxCalculationContext context)
		{
			if (_service.IsTaxExempt(context.Vehicle, context.City))
				return Money.Zero();

			return _next?.Handle(context) ?? Money.Zero();
		}

		public void SetNext(ITaxRuleHandler next) => _next = next;
	}

}

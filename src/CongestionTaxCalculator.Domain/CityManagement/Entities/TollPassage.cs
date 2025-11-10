using CongestionTaxCalculator.Domain.Common;
using CongestionTaxCalculator.Domain.TaxCalculation.Entities;
using CongestionTaxCalculator.Domain.TaxCalculation.ValueObjects;

namespace CongestionTaxCalculator.Domain.CityManagement.Entities
{
	public class TollPassage : AuditableBaseEntity<Guid>
	{
		public int CityId { get; private set; }
		public City City { get; private set; } = default!;

		public Guid VehicleId { get; private set; }
		public Vehicle Vehicle { get; private set; } = default!;

		public DateTime PassageTime { get; private set; }

		public Money? AppliedTax { get; private set; }

		private TollPassage() { }

		public TollPassage(int cityId, Guid vehicleId, DateTime passageTime)
		{
			CityId = cityId;
			VehicleId = vehicleId;
			PassageTime = passageTime;
		}

		public void SetAppliedTax(Money tax)
		{
			AppliedTax = tax;
		}
	}
}

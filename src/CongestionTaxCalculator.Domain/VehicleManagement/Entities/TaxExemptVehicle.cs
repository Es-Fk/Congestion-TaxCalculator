using CongestionTaxCalculator.Domain.Common;

namespace CongestionTaxCalculator.Domain.TaxCalculation.Entities
{
	public class TaxExemptVehicle : AuditableBaseEntity<int>
	{
		public Vehicle Vehicle { get; private set; }
		public Guid VehicleId { get; private set; }
		public TaxExemptVehicle() { }
		public TaxExemptVehicle(Vehicle vehicle)
		{
			Vehicle = vehicle;
		}
	}
}

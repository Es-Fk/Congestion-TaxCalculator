using CongestionTaxCalculator.Domain.Common;
using CongestionTaxCalculator.Domain.Entities.Enums;

namespace CongestionTaxCalculator.Domain.Entities
{
	public class TaxExemptVehicle : AuditableBaseEntity<int>
	{
		public VehicleType VehicleType { get; private set; }
		public TaxExemptVehicle() { }
		public TaxExemptVehicle(VehicleType vehicleType)
		{
			VehicleType = vehicleType;
		}
	}
}

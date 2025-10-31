using CongestionTaxCalculator.Domain.Common;
using CongestionTaxCalculator.Domain.Enums;

namespace CongestionTaxCalculator.Domain.Entities
{
	public class TaxExemptVehicle : BaseEntity<int>
	{
		public VehicleType VehicleType { get; set; }
	}
}

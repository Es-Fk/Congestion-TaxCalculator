using CongestionTaxCalculator.Domain.Common;
using CongestionTaxCalculator.Domain.Enums;

namespace CongestionTaxCalculator.Domain.Entities
{
	public class Vehicle : BaseEntity<Guid>
	{
		public string RegistrationNumber { get; set; } = default!;
		public VehicleType VehicleType { get; set; }
	}
}

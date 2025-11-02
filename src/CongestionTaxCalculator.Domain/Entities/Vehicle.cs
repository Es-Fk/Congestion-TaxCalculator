using CongestionTaxCalculator.Domain.Common;
using CongestionTaxCalculator.Domain.Entities.Enums;

namespace CongestionTaxCalculator.Domain.Entities
{
	public class Vehicle : AuditableBaseEntity<Guid>
	{
		public string RegistrationNumber { get; private set; }
		public VehicleType VehicleType { get; private set; }
		public Vehicle() {}
		public Vehicle(Guid id, VehicleType type, string reg)
		{
			Id = id;
			VehicleType = type;
			RegistrationNumber = reg;
		}
	}
}

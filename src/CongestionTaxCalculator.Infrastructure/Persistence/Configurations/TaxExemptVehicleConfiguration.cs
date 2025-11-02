using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Configurations
{
	public class TaxExemptVehicleConfiguration : AuditableBaseEntityConfiguration<TaxExemptVehicle, int>
	{
		public override void Configure(EntityTypeBuilder<TaxExemptVehicle> builder)
		{
			builder.ToTable("TaxExemptVehicles");
			builder.HasKey(ev => ev.Id);

			builder.Property(ev => ev.VehicleType)
				   .IsRequired()
				   .HasConversion<int>();
			builder.HasData(
				new { Id = 1, VehicleType = VehicleType.Motorcycle, CreatedOn = DateTime.UtcNow },
				new { Id = 2, VehicleType = VehicleType.Bus, CreatedOn = DateTime.UtcNow },
				new { Id = 3, VehicleType = VehicleType.Emergency, CreatedOn = DateTime.UtcNow },
				new { Id = 4, VehicleType = VehicleType.Military, CreatedOn = DateTime.UtcNow },
				new { Id = 5, VehicleType = VehicleType.Diplomat, CreatedOn = DateTime.UtcNow },
				new { Id = 6, VehicleType = VehicleType.Foreign, CreatedOn = DateTime.UtcNow }
			);
		}
	}
}

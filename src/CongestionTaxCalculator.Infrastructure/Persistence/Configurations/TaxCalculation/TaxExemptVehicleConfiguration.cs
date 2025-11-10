using CongestionTaxCalculator.Domain.TaxCalculation.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Configurations.TaxCalculation
{
	public class TaxExemptVehicleConfiguration : AuditableBaseEntityConfiguration<TaxExemptVehicle, int>
	{
		public override void Configure(EntityTypeBuilder<TaxExemptVehicle> builder)
		{
			builder.ToTable("TaxExemptVehicles");
			builder.HasKey(ev => ev.Id);
			builder.Property(tr => tr.Id).ValueGeneratedOnAdd();

			builder.HasOne(te => te.Vehicle)
				   .WithMany()
				   .HasForeignKey(te => te.VehicleId)
				   .OnDelete(DeleteBehavior.Cascade);


			builder.HasData(
				new { Id = 1, RowVersion = Guid.NewGuid().ToByteArray(), CityId = 1, VehicleId = Guid.Parse("00000000-0000-0000-0000-000000000002"), CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc) },
				new { Id = 2, RowVersion = Guid.NewGuid().ToByteArray(), CityId = 1, VehicleId = Guid.Parse("00000000-0000-0000-0000-000000000003"), CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc) },
				new { Id = 3, RowVersion = Guid.NewGuid().ToByteArray(), CityId = 1, VehicleId = Guid.Parse("00000000-0000-0000-0000-000000000004"), CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc) },
				new { Id = 4, RowVersion = Guid.NewGuid().ToByteArray(), CityId = 1, VehicleId = Guid.Parse("00000000-0000-0000-0000-000000000005"), CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc) },
				new { Id = 5, RowVersion = Guid.NewGuid().ToByteArray(), CityId = 1, VehicleId = Guid.Parse("00000000-0000-0000-0000-000000000006"), CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc) },
				new { Id = 6, RowVersion = Guid.NewGuid().ToByteArray(), CityId = 1, VehicleId = Guid.Parse("00000000-0000-0000-0000-000000000007"), CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc) }
			);
		}
	}
}

using CongestionTaxCalculator.Domain.TaxCalculation.Entities;
using CongestionTaxCalculator.Domain.VehicleManagement.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Configurations.VehicleManagement
{
	public class VehicleConfiguration : AuditableBaseEntityConfiguration<Vehicle, Guid>
	{
		public override void Configure(EntityTypeBuilder<Vehicle> builder)
		{
			base.Configure(builder);
			builder.ToTable("Vehicles");
			builder.HasKey(v => v.Id);
			builder.Property(v => v.Id).ValueGeneratedOnAdd();
			builder.Property(v => v.RegistrationNumber)
				   .IsRequired()
				   .HasMaxLength(20);
			builder.Property(v => v.VehicleType)
				   .IsRequired()
				   .HasConversion<int>();

			builder.HasData(
				new { Id =Guid.Parse("00000000-0000-0000-0000-000000000001"), VehicleType = VehicleType.Car, RegistrationNumber = "ABC123", CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc) },
				new { Id =Guid.Parse("00000000-0000-0000-0000-000000000002"), VehicleType = VehicleType.Diplomat, RegistrationNumber = "DEF456", CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc) },
				new { Id =Guid.Parse("00000000-0000-0000-0000-000000000003"), VehicleType = VehicleType.Motorcycle, RegistrationNumber = "GHI789", CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc) },
				new { Id =Guid.Parse("00000000-0000-0000-0000-000000000004"), VehicleType = VehicleType.Emergency, RegistrationNumber = "JKL012", CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc) },
				new { Id =Guid.Parse("00000000-0000-0000-0000-000000000005"), VehicleType = VehicleType.Bus, RegistrationNumber = "MNO345", CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc) },
				new { Id =Guid.Parse("00000000-0000-0000-0000-000000000006"), VehicleType = VehicleType.Military, RegistrationNumber = "PQR678", CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc) },
				new { Id =Guid.Parse("00000000-0000-0000-0000-000000000007"), VehicleType = VehicleType.Foreign, RegistrationNumber = "STU901", CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc) }
			);
		}
	}
}

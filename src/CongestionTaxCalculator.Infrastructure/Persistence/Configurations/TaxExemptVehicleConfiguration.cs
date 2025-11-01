using CongestionTaxCalculator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Configurations
{
	public class TaxExemptVehicleConfiguration : AuditableBaseEntityConfiguration<TaxExemptVehicle,int>
    {
        public override void Configure(EntityTypeBuilder<TaxExemptVehicle> builder)
        {
            builder.ToTable("TaxExemptVehicles");
            builder.HasKey(ev => ev.Id);

            builder.Property(ev => ev.VehicleType)
                   .IsRequired()
                   .HasConversion<int>();
        }
    }
}

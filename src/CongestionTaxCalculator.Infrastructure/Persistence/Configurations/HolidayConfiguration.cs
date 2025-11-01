using CongestionTaxCalculator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Configurations
{
	public class HolidayConfiguration : AuditableBaseEntityConfiguration<Holiday,int>
	{
		public override void Configure(EntityTypeBuilder<Holiday> builder)
		{
			builder.ToTable("Holidays");
			builder.HasKey(h => h.Id);

			builder.Property(h => h.Date)
				   .IsRequired()
				   .HasConversion(
					   v => v.ToDateTime(TimeOnly.MinValue),
					   v => DateOnly.FromDateTime(v))
				   .HasColumnType("date");

			builder.Property(h => h.Description)
				   .HasMaxLength(200);
		}
	}
}

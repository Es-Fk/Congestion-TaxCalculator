using CongestionTaxCalculator.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Configurations
{
	public class HolidayConfiguration : AuditableBaseEntityConfiguration<Holiday, int>
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

			builder.HasData(
				new { Id = 1, Date = new DateOnly(2025, 1, 1), Description = "New Year’s Day", CreatedOn = DateTime.UtcNow },
				new { Id = 2, Date = new DateOnly(2025, 12, 25), Description = "Christmas Day", CreatedOn = DateTime.UtcNow }
			);
		}
	}
}

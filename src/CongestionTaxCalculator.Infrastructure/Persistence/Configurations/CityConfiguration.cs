using CongestionTaxCalculator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Configurations
{
	public class CityConfiguration : AuditableBaseEntityConfiguration<City,int>
	{
		public override void Configure(EntityTypeBuilder<City> builder)
		{
			base.Configure(builder);

			builder.ToTable("Cities");
			builder.HasKey(c => c.Id);

			builder.Property(c => c.Name)
				   .IsRequired()
				   .HasMaxLength(200);

			builder.Property(c => c.SingleChargeDurationMinutes)
				   .HasConversion<int>()
				   .HasColumnName("SingleChargeDurationMinutes");

			builder.Property(c => c.IsHolidayTaxExempt).IsRequired();
			builder.Property(c => c.IsDayBeforeHolidayTaxExempt).IsRequired();
			builder.Property(c => c.IsWeekendTaxExempt).IsRequired();
			builder.Property(c => c.IsJulyTaxExempt).IsRequired();

			builder.OwnsOne(c => c.MaximumTaxPerDay, m =>
			{
				m.Property(p => p.Amount)
				 .HasColumnName("MaximumTaxPerDayAmount")
				 .HasColumnType("decimal(18,2)")
				 .IsRequired();

				m.Property(p => p.Currency)
				 .HasColumnName("MaximumTaxPerDayCurrency")
				 .HasMaxLength(3)
				 .IsRequired();
			});

			ConfigureCollection(builder, nameof(City.TaxRules));
			ConfigureCollection(builder, nameof(City.Holidays));
			ConfigureCollection(builder, nameof(City.TaxExemptVehicles));
		}

		private static void ConfigureCollection(EntityTypeBuilder<City> builder, string navigationName)
		{
			var nav = builder.Metadata.FindNavigation(navigationName)!;
			nav.SetPropertyAccessMode(PropertyAccessMode.Field);
			builder.HasMany(navigationName)
				   .WithOne()
				   .HasForeignKey("CityId")
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}

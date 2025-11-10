using CongestionTaxCalculator.Domain.CityManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Configurations.TaxCalculation
{
	public class TollPassageConfiguration : AuditableBaseEntityConfiguration<TollPassage, Guid>
	{
		public override void Configure(EntityTypeBuilder<TollPassage> builder)
		{
			base.Configure(builder);

			builder.ToTable("TollPassages");
			builder.HasKey(tr => tr.Id);
			builder.Property(tr => tr.Id).ValueGeneratedOnAdd();


			builder.Property(tp => tp.PassageTime)
				.IsRequired();

			builder.HasIndex(tp => tp.PassageTime);
			builder.HasIndex(tp => new { tp.CityId, tp.PassageTime });

			builder.HasOne(tp => tp.City)
				.WithMany()
				.HasForeignKey(tp => tp.CityId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(tp => tp.Vehicle)
				.WithMany()
				.HasForeignKey(tp => tp.VehicleId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.OwnsOne(tp => tp.AppliedTax, taxBuilder =>
			{
				builder.Navigation(tp => tp.AppliedTax).IsRequired(false);

				taxBuilder.Property(m => m.Amount)
					.HasColumnName("AppliedTax_Amount")
					.HasColumnType("decimal(18,2)");

				taxBuilder.Property(m => m.Currency)
					.HasColumnName("AppliedTax_Currency")
					.HasMaxLength(10)
					.IsRequired(false);
			});
		}
	}
}

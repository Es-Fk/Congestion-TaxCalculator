using CongestionTaxCalculator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Configurations
{
	public class TaxRuleConfiguration : AuditableBaseEntityConfiguration<TaxRule, int>
	{
		public override void Configure(EntityTypeBuilder<TaxRule> builder)
		{
			builder.ToTable("TaxRules");
			builder.HasKey(tr => tr.Id);
			builder.Property(tr => tr.Id).ValueGeneratedOnAdd();

			builder.Property(tr => tr.StartTime)
				   .IsRequired();

			builder.Property(tr => tr.EndTime)
				   .IsRequired();

			builder.OwnsOne(tr => tr.Amount, m =>
			{
				m.Property(p => p.Amount)
				 .HasColumnName("AmountValue")
				 .HasColumnType("decimal(18,2)");

				m.Property(p => p.Currency)
				 .HasColumnName("AmountCurrency")
				 .HasMaxLength(3);
			});

			builder.HasData(
				new
				{
					Id = 1,
					CityId = 1,
					StartTime = new TimeSpan(6, 0, 0),
					EndTime = new TimeSpan(6, 30, 0),
					CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc),
					RowVersion = Guid.NewGuid().ToByteArray()
				},
				new
				{
					Id = 2,
					CityId = 1,
					StartTime = new TimeSpan(6, 30, 0),
					EndTime = new TimeSpan(7, 0, 0),
					CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc),
					RowVersion = Guid.NewGuid().ToByteArray()
				},
				new
				{
					Id = 3,
					CityId = 1,
					StartTime = new TimeSpan(7, 0, 0),
					EndTime = new TimeSpan(8, 0, 0),
					CreatedOn = new DateTime(2025, 11, 2, 12, 32, 14, DateTimeKind.Utc),
					RowVersion = Guid.NewGuid().ToByteArray()
				}
			);

			builder.OwnsOne(c => c.Amount).HasData(
				new { TaxRuleId = 1, Amount = 8m, Currency = "SEK" },
				new { TaxRuleId = 2, Amount = 13m, Currency = "SEK" },
				new { TaxRuleId = 3, Amount = 18m, Currency = "SEK" }
			);
		}
	}
}

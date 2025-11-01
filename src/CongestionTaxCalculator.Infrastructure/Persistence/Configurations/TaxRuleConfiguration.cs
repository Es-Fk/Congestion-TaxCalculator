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
		}
	}
}

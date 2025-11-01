using CongestionTaxCalculator.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Configurations
{
	public abstract class AuditableBaseEntityConfiguration<TBase, TKey> : IEntityTypeConfiguration<TBase>
		where TBase : AuditableBaseEntity<TKey>
	{
		public virtual void Configure(EntityTypeBuilder<TBase> builder)
		{
			builder.Property(e => e.CreatedOn)
				   .IsRequired()
				   .HasColumnType("datetime2")
				   .ValueGeneratedOnAdd()
				   .HasDefaultValueSql("SYSUTCDATETIME()");

			builder.Property(e => e.ModifiedOn)
				   .HasColumnType("datetime2")
				   .IsRequired(false);

			builder.Property(e => e.CreatedBy)
				   .HasMaxLength(256)
				   .IsRequired(false);

			builder.Property(e => e.ModifiedBy)
				   .HasMaxLength(256)
				   .IsRequired(false);
			
			builder.Property(e => e.RowVersion)
				   .IsRowVersion()
				   .IsRequired(false)
				   .HasColumnType("rowversion")
				   .ValueGeneratedOnAddOrUpdate();
		}
	}
}

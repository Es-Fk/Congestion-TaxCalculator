using CongestionTaxCalculator.Domain.Common.Interfaces;

namespace CongestionTaxCalculator.Domain.Common
{
	public abstract class AuditableBaseEntity<TKey> : BaseEntity<TKey>, IAuditableEntity
	{
		public DateTime CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public string? CreatedBy { get; set; }
		public string? ModifiedBy { get; set; }
	}
}

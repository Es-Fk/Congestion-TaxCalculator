using System.ComponentModel.DataAnnotations;

namespace CongestionTaxCalculator.Domain.Common
{
	public abstract class BaseEntity<TKey> : IEntity<TKey>, IHasRowVersion
	{
		public TKey Id { get; set; } = default!;
		[Timestamp]
		public byte[] RowVersion { get; set; } = default!;
		public void SetRowVersion(byte[] rowVersion)
		{
			RowVersion = rowVersion;
		}
	}
}

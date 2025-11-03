using System.ComponentModel.DataAnnotations;

namespace CongestionTaxCalculator.Domain.Common
{
    public interface IHasRowVersion
    {
        [Timestamp]
        byte[] RowVersion { get; }
	}
}

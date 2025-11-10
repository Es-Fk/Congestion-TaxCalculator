using System.ComponentModel.DataAnnotations;

namespace CongestionTaxCalculator.Domain.Common.Interfaces
{
    public interface IHasRowVersion
    {
        [Timestamp]
        byte[]? RowVersion { get; }
	}
}

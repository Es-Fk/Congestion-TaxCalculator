using System.ComponentModel.DataAnnotations;

namespace CongestionTaxCalculator.Domain.Common
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
	}
}

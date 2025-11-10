using System.ComponentModel.DataAnnotations;

namespace CongestionTaxCalculator.Domain.Common.Interfaces
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
	}
}

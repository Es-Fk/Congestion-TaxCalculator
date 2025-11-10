namespace CongestionTaxCalculator.Domain.Common.Interfaces
{
	public interface IDomainEvent
	{
		DateTime OccurredOn { get; }
	}
}

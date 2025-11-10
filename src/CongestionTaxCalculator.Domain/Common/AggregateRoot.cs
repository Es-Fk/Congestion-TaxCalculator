using CongestionTaxCalculator.Domain.Common.Interfaces;

namespace CongestionTaxCalculator.Domain.Common
{
	public abstract class AggregateRoot<TId> : AuditableBaseEntity<TId>
	{
		private readonly List<IDomainEvent> _domainEvents = new();

		protected AggregateRoot() { }

		public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

		protected void AddDomainEvent(IDomainEvent domainEvent)
		{
			_domainEvents.Add(domainEvent);
		}

		public void ClearDomainEvents()
		{
			_domainEvents.Clear();
		}
	}
}

using CongestionTaxCalculator.Domain.Common;

namespace CongestionTaxCalculator.Domain.Entities
{
	public class Holiday : AuditableBaseEntity<int>
    {
        public DateOnly Date { get;private set; }
        public string? Description { get; private set; }
        public Holiday() { }
        public Holiday(DateOnly date, string? description = null)
        {
            Date = date;
            Description = description;
		}
	}
}

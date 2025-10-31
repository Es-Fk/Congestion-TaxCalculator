using CongestionTaxCalculator.Domain.Common;

namespace CongestionTaxCalculator.Domain.Entities
{
	public class Holiday : BaseEntity<int>
    {
        public DateOnly Date { get; set; }
        public string? Description { get; set; }
    }
}

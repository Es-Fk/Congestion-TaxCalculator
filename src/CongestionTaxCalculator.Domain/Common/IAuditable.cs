namespace CongestionTaxCalculator.Domain.Common
{
    public interface IAuditableEntity
    {
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}

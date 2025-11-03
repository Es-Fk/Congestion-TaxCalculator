namespace CongestionTaxCalculator.Api.Dtos
{
	public class CalculateTaxResponseDto
	{
		public decimal TotalTax { get; set; }
		public string Currency { get; set; } = null!;
	}
}

namespace CongestionTaxCalculator.Application.Dtos
{
	public record CalculateCongestionTaxResult(
		string CityName,
		string VehicleRegistration,
		decimal TotalTax,
		int PassageCount
	);
}

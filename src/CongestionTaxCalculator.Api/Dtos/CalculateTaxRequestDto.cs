using CongestionTaxCalculator.Domain.VehicleManagement.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace CongestionTaxCalculator.Api.Dtos
{
	public class CalculateTaxRequestDto
	{
		[Required(ErrorMessage = "City name is required.")]
		[MaxLength(60,ErrorMessage ="{0} cannot be more than {1} characters")]
		public string CityName { get; set; } = null!;

		[Required(ErrorMessage = "Vehicle type is required.")]
		public VehicleType VehicleType { get; set; }

		[Required(ErrorMessage = "At least one passage time is required.")]
		[MinLength(1, ErrorMessage = "At least one passage time must be provided.")]
		public List<DateTime> PassageTimes { get; set; } = new();
	}

}

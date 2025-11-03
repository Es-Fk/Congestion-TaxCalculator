using CongestionTaxCalculator.Api.Dtos;
using CongestionTaxCalculator.Domain.Entities.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace CongestionTaxCalculator.Api.Examples
{
	public class AllSingleVehicleExamples : IMultipleExamplesProvider<CalculateTaxRequestDto>
	{
		public IEnumerable<SwaggerExample<CalculateTaxRequestDto>> GetExamples()
		{
			yield return SwaggerExample.Create("NormalCar", new NormalCarExample().GetExamples());
			yield return SwaggerExample.Create("ExemptVehicle", new ExemptVehicleExample().GetExamples());
			yield return SwaggerExample.Create("HolidayOrJuly", new HolidayOrJulyExample().GetExamples());
			yield return SwaggerExample.Create("MultiplePasses", new MultiplePassesExample().GetExamples());
		}
	}
	public class NormalCarExample 
	{
		public CalculateTaxRequestDto GetExamples() =>
			new CalculateTaxRequestDto
			{
				CityName = "Gothenburg",
				VehicleType = VehicleType.Car,
				PassageTimes = new List<DateTime>
				{
					new DateTime(2013, 2, 7, 6, 15, 0),
					new DateTime(2013, 2, 7, 15, 10, 0)
				}
			};
	}

	public class ExemptVehicleExample 
	{
		public CalculateTaxRequestDto GetExamples() =>
			new CalculateTaxRequestDto
			{
				CityName = "Gothenburg",
				VehicleType = VehicleType.Motorcycle,
				PassageTimes = new List<DateTime>
				{
					new DateTime(2013, 2, 8, 6, 27, 0),
					new DateTime(2013, 2, 8, 16, 0, 0)
				}
			};
	}

	public class HolidayOrJulyExample 
	{
		public CalculateTaxRequestDto GetExamples() =>
			new CalculateTaxRequestDto
			{
				CityName = "Gothenburg",
				VehicleType = VehicleType.Car,
				PassageTimes = new List<DateTime>
				{
					new DateTime(2013, 7, 10, 8, 30, 0),
					new DateTime(2013, 7, 10, 17, 45, 0)
				}
			};
	}

	public class MultiplePassesExample 
	{
		public CalculateTaxRequestDto GetExamples() =>
			new CalculateTaxRequestDto
			{
				CityName = "Gothenburg",
				VehicleType = VehicleType.Car,
				PassageTimes = new List<DateTime>
				{
					new DateTime(2013, 2, 8, 6, 0, 0),
					new DateTime(2013, 2, 8, 6, 20, 0),
					new DateTime(2013, 2, 8, 6, 45, 0),
					new DateTime(2013, 2, 8, 7, 15, 0)
				}
			};
	}
}

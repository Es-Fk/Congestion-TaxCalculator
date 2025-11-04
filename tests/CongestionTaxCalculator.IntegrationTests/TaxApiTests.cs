using CongestionTaxCalculator.Api.Dtos;
using CongestionTaxCalculator.Domain.Entities.Enums;
using System.Net.Http.Json;

namespace CongestionTaxCalculator.IntegrationTests
{
	public class TaxApiTests : IClassFixture<CustomWebApplicationFactory>
	{
		private readonly CustomWebApplicationFactory _factory;

		public TaxApiTests(CustomWebApplicationFactory factory) => _factory = factory;

		[Fact]
		public async Task CalculateTax_MultiplePasses_ReturnsExpectedTotal()
		{
			using var client = _factory.CreateClient();

			var request = new CalculateTaxRequestDto
			{
				CityName = "Gothenburg",
				VehicleType = VehicleType.Car,
				PassageTimes = new List<DateTime>
				{
					new DateTime(2013,2,8,6,0,0),
					new DateTime(2013,2,8,6,20,0),
					new DateTime(2013,2,8,6,45,0),
					new DateTime(2013,2,8,7,15,0)
				}
			};

			var response = await client.PostAsJsonAsync("api/tax/calc", request);
			response.EnsureSuccessStatusCode();

			var dto = await response.Content.ReadFromJsonAsync<CalculateTaxResponseDto>();
			Assert.NotNull(dto);
			Assert.Equal(31m, dto!.TotalTax);
			Assert.Equal("SEK", dto.Currency);
		}

		[Fact]
		public async Task CalculateTax_ExemptVehicle_ReturnsZero()
		{
			using var client = _factory.CreateClient();

			var request = new CalculateTaxRequestDto
			{
				CityName = "Gothenburg",
				VehicleType = VehicleType.Motorcycle,
				PassageTimes = new List<DateTime>
				{
					new DateTime(2013,2,8,8,30,0),
					new DateTime(2013,2,8,9,15,0)
				}
			};

			var response = await client.PostAsJsonAsync("api/tax/calc", request);
			response.EnsureSuccessStatusCode();

			var dto = await response.Content.ReadFromJsonAsync<CalculateTaxResponseDto>();
			Assert.NotNull(dto);
			Assert.Equal(0m, dto!.TotalTax);
			Assert.Equal("SEK", dto.Currency);
		}

	}
}

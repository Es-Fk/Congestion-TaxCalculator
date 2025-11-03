using AutoMapper;
using CongestionTaxCalculator.Api.Dtos;
using CongestionTaxCalculator.Application.Features.CalculateCongestionTax.Commands;

namespace CongestionTaxCalculator.Api.Mappers
{
	 public class CongestionTaxMappingProfile : Profile
    {
        public CongestionTaxMappingProfile()
        {
            CreateMap<CalculateTaxRequestDto, CalculateCongestionTaxCommand>().ReverseMap();
        }
    }
}

using AutoMapper;
using CongestionTaxCalculator.Api.Dtos;
using CongestionTaxCalculator.Application.Features.CalculateCongestionTax.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CongestionTaxCalculator.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TaxController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public TaxController(IMediator mediator, IMapper mapper)
		{
			_mediator = mediator;
			_mapper = mapper;
		}

		[HttpPost("calc")]
		[ProducesResponseType(typeof(CalculateTaxResponseDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CalculateTax([FromBody] CalculateTaxRequestDto taxRequestDto, CancellationToken ct)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var command = _mapper.Map<CalculateCongestionTaxCommand>(taxRequestDto);
			var result = await _mediator.Send(command, ct);
			return Ok(new CalculateTaxResponseDto { TotalTax = result?.Amount ?? 0, Currency = result?.Currency ?? "" });
		}
	}
}

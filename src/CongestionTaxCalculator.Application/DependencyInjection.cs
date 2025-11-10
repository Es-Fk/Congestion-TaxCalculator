using CongestionTaxCalculator.Application.Common.Behaviors;
using CongestionTaxCalculator.Domain.CityManagement.Services.Interfaces;
using CongestionTaxCalculator.Domain.TaxCalculation.DomainServices;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CongestionTaxCalculator.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			
			services.AddScoped<ICongestionTaxCalculatorService,CongestionTaxCalculatorService>();
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
				cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
			});
			return services;
		}
	}
}

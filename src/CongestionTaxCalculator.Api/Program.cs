using CongestionTaxCalculator.Api.Middlewares;
using CongestionTaxCalculator.Application;
using CongestionTaxCalculator.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text.Json.Serialization;

//for desing time migrations
var configuration = new ConfigurationBuilder()
		  .SetBasePath(Directory.GetCurrentDirectory())
		  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		  .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
		  .AddEnvironmentVariables()
		  .Build();


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
	options.AddPolicy("Allow*",
		builder => builder
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Congestion Tax Calculator API",
		Version = "v1",
		Description = "API for congestion tax calculator"
	});
	c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

builder.Services.AddControllers()
	.ConfigureApiBehaviorOptions(options =>
	{
		options.InvalidModelStateResponseFactory = context =>
		{
			var errors = context.ModelState
				.Where(e => e.Value.Errors.Count > 0)
				.ToDictionary(
					kvp => kvp.Key,
					kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
				);

			var problemDetails = new ValidationProblemDetails(errors)
			{
				Title = "Invalid request",
				Status = StatusCodes.Status400BadRequest,
				Detail = "One or more validation errors occurred during model binding.",
				Instance = context.HttpContext.Request.Path
			};

			problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

			return new BadRequestObjectResult(problemDetails)
			{
				ContentTypes = { "application/problem+json" }
			};
		};
	})
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});


builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<ExceptionHandling>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddInfrastructure(configuration);

builder.Services.AddApplicationServices();


var app = builder.Build();

app.UseExceptionHandler();

app.InitializeDatabase();

app.UseCors("Allow*");

app.UseRouting();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "Congestion Tax Calculator v1");
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program { }


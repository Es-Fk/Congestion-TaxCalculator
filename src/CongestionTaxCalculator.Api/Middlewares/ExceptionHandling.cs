using FluentValidation.Results;
using CongestionTaxCalculator.Infrastructure.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace CongestionTaxCalculator.Api.Middlewares
{
	public class ExceptionHandling : IExceptionHandler
	{
		private readonly ILogger<ExceptionHandling> _logger;

		public ExceptionHandling(ILogger<ExceptionHandling> logger)
		{
			_logger = logger;
		}

		public async ValueTask<bool> TryHandleAsync(
			HttpContext httpContext,
			System.Exception exception,
			CancellationToken cancellationToken)
		{
			if (httpContext is null || exception is null)
			{
				return false;
			}

			if (httpContext.Response.HasStarted)
			{
				_logger.LogWarning("Response has already started. Cannot handle exception for request {Path}", httpContext.Request.Path);
				return false;
			}

			var env = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
			var isDevelopment = env.IsDevelopment();

			var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

			_logger.LogError(exception, "An unhandled exception occurred (TraceId: {TraceId}): {Message}", traceId, exception.Message);

			ProblemDetails problemDetails;

			switch (exception)
			{
				case NotFoundException notFound:
				{
					problemDetails = new ProblemDetails
					{
						Title = "Resource not found",
						Status = StatusCodes.Status404NotFound,
						Detail = notFound.Message,
						Instance = httpContext.Request.Path
					};
					break;
				}
				case FluentValidation.ValidationException validationException:
				{
					var errors = validationException.Errors
						.GroupBy(e => e.PropertyName)
						.ToDictionary(
							g => string.IsNullOrWhiteSpace(g.Key) ? string.Empty : g.Key,
							g => g.Select(e => e.ErrorMessage).Distinct().ToArray()
						);

					var validationProblem = new ValidationProblemDetails(errors)
					{
						Title = "Validation failed",
						Status = StatusCodes.Status400BadRequest,
						Detail = "One or more validation errors occurred.",
						Instance = httpContext.Request.Path
					};

					problemDetails = validationProblem;
					break;
				}
				default:
				{
					problemDetails = new ProblemDetails
					{
						Title = "An unexpected error occurred",
						Status = StatusCodes.Status500InternalServerError,
						Detail = isDevelopment ? exception.ToString() : "An internal server error has occurred. Please try again later.",
						Instance = httpContext.Request.Path
					};
					break;
				}
			}

			if (!problemDetails.Extensions.ContainsKey("traceId"))
			{
				problemDetails.Extensions["traceId"] = traceId;
			}

			httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
			httpContext.Response.ContentType = "application/problem+json";

			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

			return true;
		}
	}
}

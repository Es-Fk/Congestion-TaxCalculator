using CongestionTaxCalculator.Infrastructure;
using Serilog;

var configuration = new ConfigurationBuilder()
		  .SetBasePath(Directory.GetCurrentDirectory())
		  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		  .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
		  .AddEnvironmentVariables()
		  .Build();


Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.Enrich.FromLogContext()
	.CreateBootstrapLogger();

try
{
	Log.Information("Application starting...");

	var builder = WebApplication.CreateBuilder(args);

	builder.Host.UseSerilog((hostContext, services, loggerConfiguration) =>
	{
		loggerConfiguration
			.ReadFrom.Configuration(hostContext.Configuration)
			.Enrich.FromLogContext();
	});

	builder.Services.AddInfrastructure(configuration);

	builder.Services.AddControllers();

	builder.Services.AddEndpointsApiExplorer();

	builder.Services.AddSwaggerGen();

	var app = builder.Build();

	if (app.Environment.IsDevelopment())
	{
		app.UseDeveloperExceptionPage();
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.InitializeDatabase();

	app.UseHttpsRedirection();

	app.UseRouting();

	app.UseAuthorization();

	app.MapControllers();

	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
	Log.CloseAndFlush();
}

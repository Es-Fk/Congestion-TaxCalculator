\# Congestion Tax Calculator API



A \*\*Domain-Driven Design (DDD)\*\* based \*\*ASP.NET Core 8 Web API\*\* following \*\*Clean Architecture\*\* principles to calculate congestion tax for vehicles.  



The API calculates fees based on \*\*multiple passages\*\*, \*\*rush hours\*\*, and \*\*vehicle exemptions\*\*, and is fully containerized with \*\*Docker\*\* using \*\*SQL Server 2022 Linux container\*\*.  



Unit and integration tests are included for robust validation of domain and application logic.



---



\## 🔹 Features



\- Calculate congestion tax for vehicles based on type and passage times

\- Supports multiple passages per day, rush hours, and vehicle exemptions

\- Implements \*\*Domain-Driven Design (DDD)\*\* and \*\*Clean Architecture\*\*:

&nbsp; - \*\*Domain layer:\*\* Entities, Value Objects, Domain Services

&nbsp; - \*\*Application layer:\*\* Commands, Queries, Handlers (CQRS with MediatR)

&nbsp; - \*\*API layer:\*\* Controllers, Swagger endpoints

\- \*\*EF Core\*\* for persistence

\- \*\*FluentValidation\*\* for input validation

\- Asynchronous logging (Serilog recommended)

\- \*\*Swagger UI\*\* with request examples

\- \*\*Unit and integration tests\*\* included

\- \*\*Dockerized\*\* with SQL Server 2022 Linux container



---



\## Project structure (high level)



\- \`src/CongestionTaxCalculator.Api/Program.cs\` — API startup, Swagger, CORS, JSON options, database initialization.

\- \`src/CongestionTaxCalculator.Api/Middlewares/ExceptionHandling.cs\` — Global exception handler produces RFC7807 problem details.

\- \`src/CongestionTaxCalculator.Api/Examples/RequestExamples.cs\` — Example requests used by Swagger (Swashbuckle.AspNetCore.Filters).

\- \`src/CongestionTaxCalculator.Infrastructure/DependencyInjection.cs\` — Infrastructure DI, DbProvider factory and database initialization logic.

\- \`src/.../\` — Domain, Application, Infrastructure projects follow Clean Architecture layering.



---



\## Prerequisites



\- Docker Desktop (Linux containers) — required for Docker Compose scenario

\- .NET 8 SDK — required for local development and running tests

\- Minimum recommended memory for Docker: 8GB



---



\## Configuration



Key configuration entries (in \`appsettings.json\` / environment variables):



\- \`DbProvider\` — e.g. \`"SqlServer"\` (used by \`DependencyInjection.InitializeDatabase()\` to run EF Core migrations at startup)

\- \`ConnectionStrings:DefaultConnection\` — DB connection string used by EF Core

\- SQL Server Docker credentials used in docker-compose:

   - User: \`sa\`

   - Password: \`1StrongPwd!!\`

   - Database: \`CongestionTaxDb\`



When \`DbProvider\` is set to \`"SqlServer"\`, the application will attempt to run \`dbContext.Database.Migrate()\` at startup inside \`InitializeDatabase()\`.



---



\## Running with Docker Compose



From repository root:

```bash

docker-compose build

docker-compose up


What this does:

1. Builds the API Docker image

2. Starts SQL Server 2022 (Linux container)

3. Starts the Congestion Tax Calculator API container

4. Exposes ports:

   - API \& Swagger: \`8080\`

   - Optional additional port: \`8081\` (if configured)



Access Swagger UI:

http://localhost:8080/swagger/index.html



Health endpoint:

http://localhost:8080/health



Notes:

- docker-compose includes a healthcheck on SQL Server; the API starts after DB readiness.

- SQL Server uses a persistent Docker volume (e.g. \`sqlserver_data\` in compose).



Example Docker environment (docker-compose) uses:

- \`SA_PASSWORD=1StrongPwd!!\`

- \`ACCEPT_EULA=Y\`

- Ensure \`DbProvider\` in appsettings or environment is set to \`SqlServer\` for migrations.

---



\## Running locally (no Docker)



1. Update \`appsettings.Development.json\` (or environment) with:

   - \`DbProvider = "SqlServer"\` (or other provider if configured)

   - \`ConnectionStrings:DefaultConnection\` (point to a reachable database)

2. Run migrations (if using SQL Server):

   - \`dotnet ef database update --project src/CongestionTaxCalculator.Infrastructure --startup-project src/CongestionTaxCalculator.Api\`

   - Or run the API and let it perform migrations automatically when \`DbProvider == "SqlServer"\`.

3. Start API:

   - \`cd src/CongestionTaxCalculator.Api\`

   - \`dotnet run\`



Swagger will be available at \`https://localhost:<port>/swagger/index.html\` (Program.cs config exposes \`/swagger/v1/swagger.json\` and UI).



---



\## API Usage



Endpoint: \`POST /api/tax/calc\`



Request DTO (example, matches \`CalculateTaxRequestDto\` and \`RequestExamples\`):

```json
{

  "vehicle": "Car",

  "dates": [

    "2013-01-14T06:00:00",

    "2013-01-14T06:20:00",

    "2013-01-14T07:15:00",

    "2013-01-14T08:05:00",

    "2013-01-14T15:30:00",

    "2013-01-14T16:10:00",

    "2013-01-14T17:25:00",

    "2013-01-14T18:45:00"

  ]

}
```



The API returns the total congestion tax amount for the vehicle in the specified city and dates.

Swagger includes multiple request examples (defined in \`src/CongestionTaxCalculator.Api/Examples/RequestExamples.cs\`), including:

\- \`NormalCar\`

\- \`ExemptVehicle\` (e.g. \`Motorcycle\`)

\- \`HolidayOrJuly\` (exempt days or months)

\- \`MultiplePasses\` (grouped passages within time windows)



---



\## Error handling & validation



\- Global exception handling is implemented in \`ExceptionHandling\` middleware. It returns RFC7807 Problem Details JSON responses, with:

  - 400 for validation errors (FluentValidation)

  - 404 for \`NotFoundException\` (application-level)

  - 500 for unexpected errors (includes stack trace in development)

\- Model validation responses are customized in \`Program.cs\` to return \`application/problem+json\`.



---



\## Tests



Run unit and integration tests:

```bash

dotnet test

```

Refer to test projects in the \`tests/\` folder (if present) for individual test commands and configurations.



---



\## Logging



The solution is prepared to use Serilog (or other logging providers). Adjust logging configuration in \`appsettings.{Environment}.json\`.



---



\## Contributing



\- Follow Clean Architecture boundaries.

\- Add tests for new behavior (unit + integration when persistence or application-level behavior changes).

\- Keep API contracts backward compatible; add new endpoints for breaking changes.



---



\## License



This project is MIT licensed. See \`LICENSE.txt\`.



---



\## Acknowledgements



\- Built with .NET 8, Entity Framework Core, MediatR, FluentValidation, and Swashbuckle (Swagger).

\- Request examples use \`Swashbuckle.AspNetCore.Filters\`.


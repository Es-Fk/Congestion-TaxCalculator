# 🚗 Congestion Tax Calculator API

A **Domain-Driven Design (DDD)** based **ASP.NET Core 8 Web API** following **Clean Architecture** principles — designed to calculate congestion tax for vehicles.

This API computes tax fees based on **multiple passages**, **rush hours**, and **vehicle exemptions**, and is fully containerized using **Docker** with **SQL Server 2022 (Linux)**.
Unit and integration tests are included to ensure reliable and consistent behavior.

---

## 🔹 Features

✅ Calculate congestion tax for vehicles based on type and passage times
✅ Handle multiple daily passages, rush hours, and vehicle exemptions
✅ Implements **DDD** + **Clean Architecture** principles:

* **Domain Layer** → Entities, Value Objects, Domain Services
* **Application Layer** → Commands, Queries, Handlers (CQRS with MediatR)
* **API Layer** → Controllers, Swagger Endpoints

✅ **EF Core** for persistence
✅ **FluentValidation** for input validation
✅ Asynchronous logging (via **Serilog**)
✅ **Swagger UI** with request examples
✅ **Unit and integration tests** included
✅ **Dockerized** with SQL Server 2022 Linux container

---

## 🧱 Project Structure (High Level)

```
src/
├── CongestionTaxCalculator.Api/
│   ├── Program.cs → API startup, Swagger, CORS, JSON options, DB init
│   ├── Middlewares/ExceptionHandling.cs → Global exception handler (RFC7807)
│   └── Examples/RequestExamples.cs → Swagger request examples
│
├── CongestionTaxCalculator.Infrastructure/
│   └── DependencyInjection.cs → Infrastructure DI, DbProvider factory, DB init
│
└── Domain & Application Layers → Follow Clean Architecture separation
```

---

## ⚙️ Prerequisites

* **Docker Desktop** (Linux containers) — required for Docker Compose
* **.NET 8 SDK** — required for local development
* **Minimum 8 GB RAM** recommended for Docker

---

## 🔧 Configuration

Key settings (`appsettings.json` or environment variables):

* `DbProvider` — e.g. `"SqlServer"`
* `ConnectionStrings:DefaultConnection` — EF Core DB connection string
* SQL Server credentials in Docker Compose:

  * **User:** `sa`
  * **Password:** `1StrongPwd!!`
  * **Database:** `CongestionTaxDb`

When `DbProvider` is set to `SqlServer`, migrations run automatically at startup inside `InitializeDatabase()`.

---

## 🐳 Running with Docker Compose

From repository root:

```bash
docker-compose build
docker-compose up
```

**What this does:**

1. Builds the API Docker image
2. Starts SQL Server 2022 (Linux container)
3. Starts the API container
4. Exposes ports:

   * **8080** → API & Swagger
   * **8081** → Optional secondary port

**Access Swagger UI:**
👉 [http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html)

**Health Endpoint:**
👉 [http://localhost:8080/health](http://localhost:8080/health)

**Notes:**

* API starts **after SQL Server is healthy** (healthcheck in compose).
* SQL Server uses a persistent Docker volume (`sqlserver_data`).

---

## 🧪 Running Locally (Without Docker)

1. Update `appsettings.Development.json`:

   * `DbProvider = "SqlServer"`
   * `ConnectionStrings:DefaultConnection` → your DB
2. Run migrations (if using SQL Server):

   ```bash
   dotnet ef database update \
     --project src/CongestionTaxCalculator.Infrastructure \
     --startup-project src/CongestionTaxCalculator.Api
   ```
3. Start API:

   ```bash
   cd src/CongestionTaxCalculator.Api
   dotnet run
   ```

**Swagger:**
[https://localhost](https://localhost):<port>/swagger/index.html

---

## 📡 API Usage

**Endpoint:** `POST /api/tax/calc`

### Example Request

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

The API returns the **total congestion tax amount** for the given vehicle and dates.

### Available Swagger Examples

* `NormalCar`
* `ExemptVehicle` (e.g. `Motorcycle`)
* `HolidayOrJuly` (exempt days/months)
* `MultiplePasses` (grouped time windows)

---

## 🚨 Error Handling & Validation

* Global exception handling in `ExceptionHandling` middleware:

  * **400** → Validation errors (FluentValidation)
  * **404** → `NotFoundException`
  * **500** → Unexpected errors (includes stack trace in Development)
* Validation responses follow RFC7807 (`application/problem+json`).

---

## 🧭 Tests

Run all unit and integration tests:

```bash
dotnet test
```

Refer to `tests/` folder for configurations and detailed examples.

---

## 🪵 Logging

The solution supports **Serilog** (or any provider).
Edit logging configuration in `appsettings.{Environment}.json`.

---

## 🤝 Contributing

* Respect Clean Architecture boundaries.
* Add **unit + integration tests** for new features.
* Keep API contracts backward-compatible.
* For breaking changes, create new endpoints instead of modifying existing ones.

---

## 📄 License

This project is **MIT licensed**. See `LICENSE.txt` for details.

---

## 💡 Acknowledgements

* Built with **.NET 8**, **Entity Framework Core**, **MediatR**, **FluentValidation**, and **Swashbuckle (Swagger)**.
* Swagger request examples provided via `Swashbuckle.AspNetCore.Filters`.

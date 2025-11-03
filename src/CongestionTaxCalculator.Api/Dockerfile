# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CongestionTaxCalculator.Api/CongestionTaxCalculator.Api.csproj", "src/CongestionTaxCalculator.Api/"]
RUN dotnet restore "src/CongestionTaxCalculator.Api/CongestionTaxCalculator.Api.csproj"
COPY . .
WORKDIR "/src/src/CongestionTaxCalculator.Api"
RUN dotnet build "CongestionTaxCalculator.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CongestionTaxCalculator.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CongestionTaxCalculator.Api.dll"]

namespace CongestionTaxCalculator.Infrastructure.Persistence.DbProviders
{
	public static class DbProviderFactory
	{
		public static IDbProvider Create(string providerName)
		{
			return providerName switch
			{
				"SqlServer" => new SqlServerDbProvider(),
				"InMemory" => new InMemoryDbProvider(),
				_ => throw new NotSupportedException($"Unsupported provider: {providerName}")
			};
		}
	}
}

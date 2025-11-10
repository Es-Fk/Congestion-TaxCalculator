namespace CongestionTaxCalculator.Domain.TaxCalculation.ValueObjects
{
	public sealed record Money : IComparable<Money>
	{
		public decimal Amount { get; }
		public string Currency { get; }

		public Money(decimal amount, string currency = "SEK")
		{
			if (string.IsNullOrWhiteSpace(currency))
				throw new ArgumentException("Currency must be a non-empty string.", nameof(currency));

			Amount = amount;
			Currency = currency.ToUpperInvariant();
		}

		// Helper method to ensure both Money instances have the same currency
		private static void EnsureSameCurrency(Money a, Money b)
		{
			if (!string.Equals(a.Currency, b.Currency, StringComparison.Ordinal))
				throw new InvalidOperationException($"Cannot operate on different currencies ('{a.Currency}' vs '{b.Currency}').");
		}

		// Arithmetic operators
		public static Money Zero(string currency = "SEK") => new Money(0m, currency);

		public static Money operator +(Money a, Money b)
		{
			EnsureSameCurrency(a, b);
			return new Money(a.Amount + b.Amount, a.Currency);
		}

		public static Money operator -(Money a, Money b)
		{
			EnsureSameCurrency(a, b);
			return new Money(a.Amount - b.Amount, a.Currency);
		}

		public static Money operator -(Money a) => new Money(-a.Amount, a.Currency);

		public static Money operator *(Money a, decimal factor) => new Money(a.Amount * factor, a.Currency);
		public static Money operator *(decimal factor, Money a) => a * factor;

		public static Money operator /(Money a, decimal divisor)
		{
			if (divisor == 0m) throw new DivideByZeroException("Cannot divide Money by zero.");
			return new Money(a.Amount / divisor, a.Currency);
		}

		public static decimal operator /(Money a, Money b)
		{
			EnsureSameCurrency(a, b);
			if (b.Amount == 0m) throw new DivideByZeroException("Cannot divide by Money with zero amount.");
			return a.Amount / b.Amount;
		}

		public static Money Min(Money a, Money b)
		{
			EnsureSameCurrency(a, b);
			return a.Amount <= b.Amount ? a : b;
		}

		public static Money Max(Money a, Money b)
		{
			EnsureSameCurrency(a, b);
			return a.Amount >= b.Amount ? a : b;
		}

		// Comparison method
		public int CompareTo(Money other)
		{
			EnsureSameCurrency(this, other);
			return Amount.CompareTo(other.Amount);
		}

		// Comparison operators
		public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;
		public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;
		public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;
		public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;

		// Override ToString for better readability
		public override string ToString() => $"{Amount} {Currency}";

		// Implicit and explicit conversions
		public static implicit operator Money(decimal amount) => new Money(amount);

		public static explicit operator decimal(Money money) => money.Amount;
	}
}

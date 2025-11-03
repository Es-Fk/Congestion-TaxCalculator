using CongestionTaxCalculator.Domain.ValueObjects;

namespace CongestionTaxCalculator.Domain.UnitTests
{
	public class MoneyTests
	{
		[Fact]
		public void Constructor_EmptyCurrency_Throws()
		{
			Assert.Throws<ArgumentException>(() => new Money(1m, ""));
		}

		[Fact]
		public void ImplicitAndExplicitConversions_Work()
		{
			Money m = 5m;
			Assert.Equal(5m, m.Amount);
			decimal d = (decimal)new Money(7.5m);
			Assert.Equal(7.5m, d);
		}

		[Fact]
		public void ArithmeticOperators_SameCurrency_Work()
		{
			var a = new Money(10m, "SEK");
			var b = new Money(5m, "SEK");

			Assert.Equal(new Money(15m, "SEK"), a + b);
			Assert.Equal(new Money(5m, "SEK"), a - b);
			Assert.Equal(new Money(20m, "SEK"), a * 2m);
			Assert.Equal(new Money(5m, "SEK"), a / 2m);
		}

		[Fact]
		public void MinMax_CompareCurrencies_MustMatch()
		{
			var a = new Money(3m, "SEK");
			var b = new Money(5m, "SEK");

			Assert.Equal(a, Money.Min(a, b));
			Assert.Equal(b, Money.Max(a, b));
		}

		[Fact]
		public void DifferentCurrency_Operations_Throw()
		{
			var a = new Money(1m, "SEK");
			var b = new Money(1m, "USD");

			Assert.Throws<InvalidOperationException>(() => { var _ = a + b; });
			Assert.Throws<InvalidOperationException>(() => { var _ = Money.Min(a, b); });
		}

		[Fact]
		public void DivisionByZeroMoney_Throws()
		{
			var a = new Money(10m, "SEK");
			var zero = new Money(0m, "SEK");
			Assert.Throws<DivideByZeroException>(() => { var _ = a / zero; });
		}

		[Fact]
		public void DivideByZeroDecimal_Throws()
		{
			var a = new Money(10m, "SEK");
			Assert.Throws<DivideByZeroException>(() => { var _ = a / 0m; });
		}
	}
}

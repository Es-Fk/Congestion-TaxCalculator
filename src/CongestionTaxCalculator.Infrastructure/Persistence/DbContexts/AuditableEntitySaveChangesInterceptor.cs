using CongestionTaxCalculator.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

namespace CongestionTaxCalculator.Infrastructure.Persistence.DbContexts
{
	public sealed class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
	{
		public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
		{
			var context = eventData.Context;
			if (context != null)
			{
				UpdateAuditableProperties(context);
			}
			return base.SavingChanges(eventData, result);
		}

		public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
		{
			var context = eventData.Context;
			if (context != null)
			{
				UpdateAuditableProperties(context);
			}
			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		private static void UpdateAuditableProperties(DbContext context)
		{
			var currentUser = GetCurrentUserName();

			foreach (var entry in context.ChangeTracker.Entries())
			{
				if (entry.Entity is not BaseEntity<int>)
					continue;

				switch (entry.State)
				{
					case EntityState.Added:
						if (entry.CurrentValues.Properties.Any(p => p.Name == "CreatedBy"))
							entry.CurrentValues["CreatedBy"] = currentUser ?? string.Empty;
						break;

					case EntityState.Modified:
						if (entry.CurrentValues.Properties.Any(p => p.Name == "ModifiedOn"))
							entry.CurrentValues["ModifiedOn"] = DateTime.UtcNow;

						if (entry.CurrentValues.Properties.Any(p => p.Name == "ModifiedBy"))
							entry.CurrentValues["ModifiedBy"] = currentUser ?? string.Empty;
						break;
				}
			}
		}

		private static string GetCurrentUserName()
		{
			try
			{
				var name = ClaimsPrincipal.Current?.Identity?.Name;
				if (!string.IsNullOrWhiteSpace(name))
					return name;

				name = Thread.CurrentPrincipal?.Identity?.Name;
				if (!string.IsNullOrWhiteSpace(name))
					return name;

				name = Environment.UserName;
				if (!string.IsNullOrWhiteSpace(name))
					return name;
			}
			catch
			{
				// ..
			}
			return string.Empty;
		}
	}
}
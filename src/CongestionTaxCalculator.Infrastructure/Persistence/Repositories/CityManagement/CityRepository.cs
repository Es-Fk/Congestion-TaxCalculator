using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CongestionTaxCalculator.Domain.TaxCalculation.Entities;
using CongestionTaxCalculator.Domain.TaxCalculation.Repositories;
using CongestionTaxCalculator.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Repositories.CityManagement
{
	public class CityRepository : BaseRepository<City, int>, ICityRepository
	{
		public CityRepository(CongestionTaxDbContext dbContext) : base(dbContext)
		{
		}

		private IQueryable<City> BuildCityQuery()
		{
			return _dbContext.Cities
				.AsNoTracking()
				.Include(c => c.TaxRules!)
				.Include(c => c.Holidays!)
				.Include(c => c.TaxExemptVehicles!)
				.ThenInclude(te => te.Vehicle).AsSplitQuery();
		}

		public override async Task<City> AddAsync(City entity)
		{
			ArgumentNullException.ThrowIfNull(entity);
			await _dbContext.Cities.AddAsync(entity).ConfigureAwait(false);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);
			return entity;
		}

		public override async Task DeleteAsync(City entity)
		{
			ArgumentNullException.ThrowIfNull(entity);

			if (!_dbContext.ChangeTracker.Entries<City>().Any(e => e.Entity == entity))
			{
				_dbContext.Cities.Attach(entity);
			}

			_dbContext.Cities.Remove(entity);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);
		}

		public override async Task<bool> ExistsAsync(int id)
		{
			return await _dbContext.Cities
				.AsNoTracking()
				.AnyAsync(c => c.Id == id)
				.ConfigureAwait(false);
		}

		public override async Task<IReadOnlyList<City>> GetAllAsync()
		{
			var query = BuildCityQuery();
			var list = await query.ToListAsync().ConfigureAwait(false);
			return list;
		}

		public async Task<City?> GetByIdAsync(int id, CancellationToken ct = default)
		{
			var query = BuildCityQuery();
			return await query.FirstOrDefaultAsync(c => c.Id == id, ct).ConfigureAwait(false);
		}

		public override async Task<City?> GetByIdAsync(int id)
		{
			return await GetByIdAsync(id, CancellationToken.None).ConfigureAwait(false);
		}

		public async Task<City?> GetByNameAsync(string name, CancellationToken ct = default)
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

			name = name.Trim().ToLower();

			var query = BuildCityQuery();
			return await query.FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == name,ct).ConfigureAwait(false);
		}

		public override async Task UpdateAsync(City entity)
		{
			ArgumentNullException.ThrowIfNull(entity);

			var set = _dbContext.Cities;
			var tracked = await set.FindAsync(new object[] { entity.Id }).ConfigureAwait(false);

			if (tracked is not null)
			{
				_dbContext.Entry(tracked).CurrentValues.SetValues(entity);
			}
			else
			{
				set.Attach(entity);
				_dbContext.Entry(entity).State = EntityState.Modified;
			}

			await _dbContext.SaveChangesAsync().ConfigureAwait(false);
		}
	}
}

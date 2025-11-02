using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Repositories
{
	public class CityRepository : BaseRepository<City,int>, ICityRepository
	{
		public CityRepository(CongestionTaxDbContext dbContext) : base(dbContext)
		{
		}

		public override async Task<City> AddAsync(City entity)
		{
			if (entity is null) throw new ArgumentNullException(nameof(entity));

			await _dbContext.Set<City>().AddAsync(entity).ConfigureAwait(false);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return entity;
		}

		public override async Task DeleteAsync(City entity)
		{
			if (entity is null) throw new ArgumentNullException(nameof(entity));

			var set = _dbContext.Set<City>();

			if (!_dbContext.ChangeTracker.Entries<City>().Any(e => e.Entity == entity))
			{
				set.Attach(entity);
			}

			set.Remove(entity);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);
		}

		public override async Task<bool> ExistsAsync(int id)
		{
			return await _dbContext.Set<City>()
				.AsNoTracking()
				.AnyAsync(c => c.Id == id)
				.ConfigureAwait(false);
		}

		public override async Task<IReadOnlyList<City>> GetAllAsync()
		{
			var query = _dbContext.Set<City>()
				.AsNoTracking()
				.Include(c => c.TaxRules!)
				.Include(c => c.Holidays!)
				.Include(c => c.TaxExemptVehicles!)
				.AsSplitQuery();

			var list = await query.ToListAsync().ConfigureAwait(false);
			return list;
		}

		public async Task<City?> GetByIdAsync(int id, CancellationToken ct = default)
		{
			var query = _dbContext.Set<City>()
				.AsNoTracking()
				.Include(c => c.TaxRules!)
				.Include(c => c.Holidays!)
				.Include(c => c.TaxExemptVehicles!)
				.AsSplitQuery();

			return await query.FirstOrDefaultAsync(c => c.Id == id, ct).ConfigureAwait(false);
		}

		public override async Task<City?> GetByIdAsync(int id)
		{
			return await GetByIdAsync(id, CancellationToken.None).ConfigureAwait(false);
		}

		public override async Task UpdateAsync(City entity)
		{
			if (entity is null) throw new ArgumentNullException(nameof(entity));

			var set = _dbContext.Set<City>();

			var tracked = await set.FindAsync([entity.Id]).ConfigureAwait(false);

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

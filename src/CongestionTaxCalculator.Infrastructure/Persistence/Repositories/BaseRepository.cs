using CongestionTaxCalculator.Domain.CityManagement.Repositories;
using CongestionTaxCalculator.Domain.Common.Interfaces;
using CongestionTaxCalculator.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Repositories
{
	public class BaseRepository<T, TKey> : IRepository<T, TKey> where T : class, IEntity<TKey>
	{
		protected readonly CongestionTaxDbContext _dbContext;

		public BaseRepository(CongestionTaxDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public virtual async Task<T> AddAsync(T entity)
		{
			await _dbContext.Set<T>().AddAsync(entity);
			await _dbContext.SaveChangesAsync();
			return entity;
		}

		public virtual async Task DeleteAsync(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
			await _dbContext.SaveChangesAsync();
		}

		public virtual async Task<bool> ExistsAsync(TKey id)
		{
			return await _dbContext.Set<T>().AnyAsync(e => e.Id!.Equals(id));
		}

		public virtual async Task<IReadOnlyList<T>> GetAllAsync()
		{
			return await _dbContext.Set<T>().ToListAsync();
		}

		public virtual async Task<T?> GetByIdAsync(TKey id)
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}

		public virtual async Task UpdateAsync(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			await _dbContext.SaveChangesAsync();
		}
	}
}

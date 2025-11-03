using CongestionTaxCalculator.Domain.Common;

namespace CongestionTaxCalculator.Domain.Interfaces.Repositories
{
	public interface IRepository<T, TKey> where T : IEntity<TKey>
	{
		Task<IReadOnlyList<T>> GetAllAsync();
		Task<T?> GetByIdAsync(TKey id);
		Task<T> AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);
		Task<bool> ExistsAsync(TKey id);
	}
}

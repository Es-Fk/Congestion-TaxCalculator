using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Repositories
{
	public class TollPassageRepository : BaseRepository<TollPassage, Guid>, ITollPassageRepository
	{
		public TollPassageRepository(CongestionTaxDbContext dbContext) : base(dbContext)
		{
		}

		public async Task AddPassageAsync(TollPassage passage, CancellationToken ct = default)
		{
			if (passage is null) throw new ArgumentNullException(nameof(passage));

			_dbContext.TollPassages.Add(passage);
			await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
		}

		public async Task<IEnumerable<TollPassage>> GetPassagesForVehicleOnDateAsync(Guid vehicleId, DateTime date, CancellationToken ct = default)
		{
			var start = date.Date;
			var end = start.AddDays(1);

			var list = await _dbContext.TollPassages
				.AsNoTracking()
				.Where(p => p.VehicleId == vehicleId && p.PassageTime >= start && p.PassageTime < end)
				.OrderBy(p => p.PassageTime)
				.ToListAsync(ct)
				.ConfigureAwait(false);

			return list;
		}
	}
}

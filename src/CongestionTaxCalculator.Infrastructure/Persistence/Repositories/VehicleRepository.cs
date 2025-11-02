using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Infrastructure.Persistence.Repositories
{
	public class VehicleRepository : BaseRepository<Vehicle, Guid>, IVehicleRepository
	{
		public VehicleRepository(CongestionTaxDbContext dbContext) : base(dbContext)
		{
		}

		public async Task AddAsync(Vehicle vehicle, CancellationToken ct = default)
		{
			if (vehicle is null)
				throw new ArgumentNullException(nameof(vehicle));

			await _dbContext.Set<Vehicle>().AddAsync(vehicle, ct).ConfigureAwait(false);
			await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
		}

		public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken ct = default)
		{
			if (id == Guid.Empty)
				return null;

			return await _dbContext
				.Set<Vehicle>()
				.AsNoTracking()
				.FirstOrDefaultAsync(v => v.Id.Equals(id), ct)
				.ConfigureAwait(false);
		}
	}
}

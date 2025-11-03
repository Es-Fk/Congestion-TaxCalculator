using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Domain.Entities.Enums;
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

			await _dbContext.Vehicles.AddAsync(vehicle, ct);
			await _dbContext.SaveChangesAsync(ct);
		}

		public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken ct = default)
		{
			if (id == Guid.Empty)
				return null;

			return await _dbContext.Vehicles.AsNoTracking()
				.FirstOrDefaultAsync(v => v.Id == id, ct);
		}

		public async Task<Vehicle?> GetByTypeAsync(VehicleType type, CancellationToken ct = default)
		{
			return await _dbContext.Vehicles.AsNoTracking()
				.FirstOrDefaultAsync(v => v.VehicleType == type, ct);
		}
	}
}

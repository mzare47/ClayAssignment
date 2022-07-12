using AccessControl.Api.Data;
using AccessControl.Api.Models.Entity;
using AccessControl.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Api.Repositories
{
    public class LocksRepository : RepositoryBase<Lock>, ILocksRepository
    {
        public LocksRepository(ClayDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Lock> GetLockByIdAsync(string lId)
        {
            return await _dbContext.Locks
                                    .AsNoTracking()
                                    .FirstAsync(l => l.LockId.ToString().Equals(lId));
        }
    }
}

using AccessControl.Api.Data;
using AccessControl.Api.Models.Entity;
using AccessControl.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Api.Repositories
{
    public class LocksAccessorsRepository : ILocksAccessorsRepository
    {
        protected readonly ClayDbContext _dbContext;

        public LocksAccessorsRepository(ClayDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Lock>> GetAccessorLocksAsync(string accessorId)
        {
            return await _dbContext.LocksAccessors.
                AsNoTracking().
                Where(x => x.AccessorId == accessorId).
                Select(a => a.Lock).ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetLockAccessorsAsync(string lockId)
        {
            return await _dbContext.LocksAccessors.
                AsNoTracking().
                Where(x => x.LockId.ToString().Equals(lockId)).
                Select(a => a.Accessor).ToListAsync();
        }

        public async Task<IEnumerable<LockAccessor>> GetByAccessorIdAsync(string accessorId)
        {
            return await _dbContext.LocksAccessors.
                AsNoTracking().
                Where(x => x.AccessorId.Equals(accessorId)).ToListAsync();
        }

        public async Task<IEnumerable<LockAccessor>> GetByLockIdAsync(string lockId)
        {
            return await _dbContext.LocksAccessors.
                AsNoTracking().
                Where(x => x.LockId.ToString().Equals(lockId)).ToListAsync();
        }

        public async Task<LockAccessor> AddAsync(LockAccessor lockAccessor)
        {
            _dbContext.LocksAccessors.Add(lockAccessor);
            await _dbContext.SaveChangesAsync();
            return lockAccessor;
        }

        public async Task<IEnumerable<LockAccessor>> AddRangeAsync(IEnumerable<LockAccessor> locksAccessors)
        {
            await _dbContext.LocksAccessors.AddRangeAsync(locksAccessors);
            await _dbContext.SaveChangesAsync();
            return locksAccessors;
        }

        public async Task DeleteAsync(LockAccessor lockAccessor)
        {
            _dbContext.LocksAccessors.Remove(lockAccessor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<LockAccessor> locksAccessors)
        {
            _dbContext.LocksAccessors.RemoveRange(locksAccessors);
            await _dbContext.SaveChangesAsync();
        }

    }
}

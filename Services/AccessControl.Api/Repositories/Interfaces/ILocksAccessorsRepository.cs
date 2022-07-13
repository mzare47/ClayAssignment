using AccessControl.Api.Models.Entity;

namespace AccessControl.Api.Repositories.Interfaces
{
    public interface ILocksAccessorsRepository
    {
        Task<IEnumerable<ApplicationUser>> GetLockAccessorsAsync(string lockId);
        Task<IEnumerable<Lock>> GetAccessorLocksAsync(string accessorId);
        Task<IEnumerable<LockAccessor>> GetByAccessorIdAsync(string accessorId);
        Task<IEnumerable<LockAccessor>> GetByLockIdAsync(string lockId);
        Task<LockAccessor> AddAsync(LockAccessor lockAccessor);
        Task<IEnumerable<LockAccessor>> AddRangeAsync(IEnumerable<LockAccessor> locksAccessors);
        Task DeleteAsync(LockAccessor lockAccessor);
        Task DeleteRangeAsync(IEnumerable<LockAccessor> locksAccessors);
    }
}

using AccessControl.Api.Models.Entity;

namespace AccessControl.Api.Repositories.Interfaces
{
    public interface ILocksRepository : IAsyncRepository<Lock>
    {
        Task<Lock> GetLockByIdAsync(string lId);
    }
}

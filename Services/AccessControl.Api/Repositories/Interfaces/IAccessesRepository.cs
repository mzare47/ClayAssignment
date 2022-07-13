using AccessControl.Api.Models.Entity;

namespace AccessControl.Api.Repositories.Interfaces
{
    public interface IAccessesRepository : IAsyncRepository<Access>
    {
        public Task<IEnumerable<Access>> GetAllAccessesAsync();
    }
}

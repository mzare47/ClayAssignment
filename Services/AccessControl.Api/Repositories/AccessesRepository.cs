using AccessControl.Api.Data;
using AccessControl.Api.Models.Entity;
using AccessControl.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Api.Repositories
{
    public class AccessesRepository : RepositoryBase<Access>, IAccessesRepository
    {
        public AccessesRepository(ClayDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Access>> GetAllAccessesAsync()
        {
            return await _dbContext.Accesses.
                AsNoTracking().
                Include(a => a.Lock).
                Include(a => a.Accessor).
                ToListAsync();
        }
    }
}

using AccessControl.Api.Data;
using AccessControl.Api.Models.Entity;
using AccessControl.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Lib.Models;

namespace AccessControl.Api.Repositories
{
    public class AccessorsRepository : IAccessorsRepository
    {
        protected readonly ClayDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccessorsRepository(ClayDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<ApplicationUser> GetAccessorByIdAsync(string accessorId)
        {
            return await _dbContext.Users
                                .AsNoTracking()
                                .FirstAsync(u => u.Id.Equals(accessorId));
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAccessors()
        {
            return await _userManager.GetUsersInRoleAsync(UserRole.Accessor);
        }

        public async Task DeleteAccessorAsync(ApplicationUser accessor)
        {
            _dbContext.UserRoles.RemoveRange(_dbContext.UserRoles.Where(u => u.UserId.Equals(accessor.Id)));
            _dbContext.Users.Remove(accessor);
            await _dbContext.SaveChangesAsync();
        }
    }
}

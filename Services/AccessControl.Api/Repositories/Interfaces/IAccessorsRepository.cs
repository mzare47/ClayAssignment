using AccessControl.Api.Models.Entity;

namespace AccessControl.Api.Repositories.Interfaces
{
    public interface IAccessorsRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAccessors();
        Task<ApplicationUser> GetAccessorByIdAsync(string id);
        Task DeleteAccessorAsync(ApplicationUser accessor);
    }
}

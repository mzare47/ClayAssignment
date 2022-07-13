using WebApp.Admin.Models;

namespace WebApp.Admin.Services
{
    public interface IAccessControlService
    {
        Task<TokenDto> GetToken(LoginDto model);

        Task<IEnumerable<LockDto>> GetAllLocks();
        Task DeleteLock(string lockId);
        Task<LockDto> CreateLock(LockToAddDto model);
        Task<LockDto> EditLock(LockDto model);
        Task<LockDto> GetLockById(string lockId);

        Task<IEnumerable<AccessorDto>> GetAllAccessors();
        Task DeleteAccessor(string accessorId);
        Task CreateAccessor(AccessorToAddDto model);
        Task<AccessorDto> GetAccessorById(string accessorId);

        Task<IEnumerable<AccessDto>> GetAllAccesses();

        Task<IEnumerable<AccessorDto>> GetLockAccessors(string lockId);
        Task<IEnumerable<LockDto>> GetAccessorLocks(string accessorId);
        Task<IEnumerable<AccessorDto>> UpdateLockAccessors(string lockId, List<string> accessorIds);
        Task<IEnumerable<LockDto>> UpdateAccessorLocks(string accessorId, List<string> lockIds);
    }
}

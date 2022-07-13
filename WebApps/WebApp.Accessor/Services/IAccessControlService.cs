using Shared.Lib.Models;
using WebApp.Accessor.Models;

namespace WebApp.Accessor.Services
{
    public interface IAccessControlService
    {
        Task<TokenDto> GetToken(LoginDto model);
        Task<IEnumerable<LockDto>> GetAccessorLocks(string accessorId);
        Task<LockDto> UpdateLock(string accessorId, string lockId, int accessType);
    }
}

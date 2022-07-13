using WebApp.Admin.Models;
using Shared.Lib.Extensions;
using System.Net.Http.Headers;

namespace WebApp.Admin.Services
{
    public class AccessControlService : IAccessControlService
    {
        private readonly HttpClient _client;
        private readonly ILogger<AccessControlService> _logger;

        public AccessControlService(HttpClient client, ILogger<AccessControlService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TokenDto> GetToken(LoginDto model)
        {
            var response = await _client.PostAsJson($"/api/authenticate/login", model);
            return await response.ReadContentAs<TokenDto>();
        }

        public async Task<IEnumerable<LockDto>> GetAllLocks()
        {
            var response = await _client.GetAsync($"/api/adminlocks");
            return await response.ReadContentAs<IEnumerable<LockDto>>();
        }

        public async Task DeleteLock(string lockId)
        {
            var response = await _client.DeleteAsync($"/api/adminlocks/{lockId}");
            if (response.IsSuccessStatusCode)
                return;
            else
            {
                throw new KeyNotFoundException($"Lock with LockId: {lockId}, not found.");
            }
        }

        public async Task<LockDto> CreateLock(LockToAddDto model)
        {
            var response = await _client.PostAsJson($"/api/adminlocks", model);
            return await response.ReadContentAs<LockDto>();
        }

        public async Task<LockDto> EditLock(LockDto model)
        {
            var response = await _client.PutAsJsonAsync($"/api/adminlocks", model);
            return await response.ReadContentAs<LockDto>();
        }

        public async Task<LockDto> GetLockById(string lockId)
        {
            var response = await _client.GetAsync($"/api/adminlocks/{lockId}");
            return await response.ReadContentAs<LockDto>();
        }

        public async Task<IEnumerable<AccessorDto>> GetAllAccessors()
        {
            var response = await _client.GetAsync($"/api/adminaccessors");
            return await response.ReadContentAs<IEnumerable<AccessorDto>>();
        }

        public async Task DeleteAccessor(string accessorId)
        {
            var response = await _client.DeleteAsync($"/api/adminaccessors/{accessorId}");
            if (response.IsSuccessStatusCode)
                return;
            else
            {
                throw new KeyNotFoundException($"Accessor with AccessorId: {accessorId}, not found.");
            }
        }

        public async Task CreateAccessor(AccessorToAddDto model)
        {
            var response = await _client.PostAsJson($"/api/authenticate/register", model);
            if (response.IsSuccessStatusCode)
                return;
            else
            {
                throw new ApplicationException($"Couldn't create accessor!");
            }
        }

        public async Task<IEnumerable<AccessDto>> GetAllAccesses()
        {
            var response = await _client.GetAsync($"/api/adminaccesses");
            return await response.ReadContentAs<IEnumerable<AccessDto>>();
        }

        public async Task<IEnumerable<AccessorDto>> GetLockAccessors(string lockId)
        {
            var response = await _client.GetAsync($"/api/adminlocksaccessors/getlockaccessors/{lockId}");
            return await response.ReadContentAs<IEnumerable<AccessorDto>>();
        }

        public async Task<IEnumerable<LockDto>> GetAccessorLocks(string accessorId)
        {
            var response = await _client.GetAsync($"/api/adminlocksaccessors/getaccessorlocks/{accessorId}");
            return await response.ReadContentAs<IEnumerable<LockDto>>();
        }

        public async Task<IEnumerable<AccessorDto>> UpdateLockAccessors(string lockId, List<string> accessorIds)
        {
            var response = await _client.PatchAsJson($"/api/adminlocksaccessors/updatelockaccessors/{lockId}", accessorIds);
            return await response.ReadContentAs<IEnumerable<AccessorDto>>();
        }

        public async Task<IEnumerable<LockDto>> UpdateAccessorLocks(string accessorId, List<string> lockIds)
        {
            var response = await _client.PatchAsJson($"/api/adminlocksaccessors/updateaccessorlocks/{accessorId}", lockIds);
            return await response.ReadContentAs<IEnumerable<LockDto>>();
        }

        public async Task<AccessorDto> GetAccessorById(string accessorId)
        {
            var response = await _client.GetAsync($"/api/adminaccessors/{accessorId}");
            return await response.ReadContentAs<AccessorDto>();
        }
    }
}

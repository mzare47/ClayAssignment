using Shared.Lib.Extensions;
using Shared.Lib.Models;
using System.Net.Http.Json;
using WebApp.Accessor.Models;

namespace WebApp.Accessor.Services
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

        public async Task<IEnumerable<LockDto>> GetAccessorLocks(string accessorId)
        {
            var response = await _client.GetAsync($"/api/adminlocksaccessors/getaccessorlocks/{accessorId}");
            return await response.ReadContentAs<IEnumerable<LockDto>>();
        }

        public async Task<LockDto> UpdateLock(string accessorId, string lockId, int accessType)
        {
            var response = await _client.PutAsync($"/api/locks/{accessorId}/{lockId}/{accessType}", null);
            return await response.ReadContentAs<LockDto>();
        }


    }
}

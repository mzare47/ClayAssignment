using AccessControl.Api.Models.Entity;
using AccessControl.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Lib.Models;

namespace AccessControl.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(Roles = UserRole.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAccessesController : ControllerBase
    {
        private readonly IAccessesRepository _accessesRepository;
        private readonly ILogger<AdminAccessesController> _logger;

        public AdminAccessesController(IAccessesRepository accessesRepository, ILogger<AdminAccessesController> logger)
        {
            _accessesRepository = accessesRepository ?? throw new ArgumentNullException(nameof(accessesRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //Get Accesses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Access>>> GetAllAccesses()
        {
            var accesses = await _accessesRepository.GetAllAccessesAsync();
            _logger.LogInformation($"All accesses, retrieved.");
            return Ok(accesses);
        }

        //Get Accesses By AccessorId
        [HttpGet("[action]/{accessorId}")]
        public async Task<ActionResult<IEnumerable<Access>>> GetByAccessorId(string accessorId)
        {
            var accesses = await _accessesRepository.GetAsync(a => a.AccessorId.Equals(accessorId));
            _logger.LogInformation($"All accesses for accessor with accessId: {accessorId}, retrieved.");
            return Ok(accesses);
        }

        //Get Accesses By LockId
        [HttpGet("[action]/{lockId}")]
        public async Task<ActionResult<IEnumerable<Access>>> GetByLockId(string lockId)
        {
            var accesses = await _accessesRepository.GetAsync(a => a.LockId.ToString().Equals(lockId));
            _logger.LogInformation($"All accesses for lock with lockId: {lockId}, retrieved.");
            return Ok(accesses);
        }
    }
}

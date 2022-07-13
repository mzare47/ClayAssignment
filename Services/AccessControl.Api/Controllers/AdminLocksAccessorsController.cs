using AccessControl.Api.Models.Entity;
using AccessControl.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Lib.Models;

namespace AccessControl.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminLocksAccessorsController : ControllerBase
    {
        private readonly ILocksAccessorsRepository _locksAccessorsRepository;
        private readonly ILocksRepository _locksRepository;
        private readonly IAccessorsRepository _accessorsRepository;
        private readonly ILogger<AdminLocksAccessorsController> _logger;

        public AdminLocksAccessorsController(ILocksAccessorsRepository locksAccessorsRepository, ILocksRepository locksRepository, IAccessorsRepository accessorsRepository, ILogger<AdminLocksAccessorsController> logger)
        {
            _locksAccessorsRepository = locksAccessorsRepository ?? throw new ArgumentNullException(nameof(locksAccessorsRepository));
            _locksRepository = locksRepository ?? throw new ArgumentNullException(nameof(locksRepository));
            _accessorsRepository = accessorsRepository ?? throw new ArgumentNullException(nameof(accessorsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //Get Lock's Accessor
        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("[action]/{lockId}")]
        public async Task<ActionResult<ApplicationUser>> GetLockAccessors(string lockId)
        {
            var lockAccessors = await _locksAccessorsRepository.GetLockAccessorsAsync(lockId);
            _logger.LogInformation($"Accessors for lock with LockId: {lockId}, retrieved.");
            return Ok(lockAccessors);
        }

        //Get Accessor's Lock
        [HttpGet("[action]/{accessorId}")]
        public async Task<ActionResult<Lock>> GetAccessorLocks(string accessorId)
        {
            var accessorLocks = await _locksAccessorsRepository.GetAccessorLocksAsync(accessorId);
            _logger.LogInformation($"Locks for accessor with AccessorId: {accessorId}, retrieved.");
            return Ok(accessorLocks);
        }

        //Patch Accessor's Lock
        [Authorize(Roles = UserRole.Admin)]
        [HttpPatch("[action]/{accessorId}")]
        public async Task<ActionResult<LockAccessor>> UpdateAccessorLocks(string accessorId, [FromBody] List<string> lockIds)
        {
            await _accessorsRepository.GetAccessorByIdAsync(accessorId);

            var locks = new List<Lock>();
            foreach (var lockId in lockIds)
            {
                var lck = await _locksRepository.GetLockByIdAsync(lockId);
                locks.Add(lck);
            }

            var itemsToDelete = await _locksAccessorsRepository.GetByAccessorIdAsync(accessorId);
            if (itemsToDelete != null && itemsToDelete.Any())
            {
                await _locksAccessorsRepository.DeleteRangeAsync(itemsToDelete);
            }

            var itemsToUpdate = new List<LockAccessor>();
            if (locks.Any())
            {
                foreach (var lck in locks)
                {
                    itemsToUpdate.Add(
                        new LockAccessor
                        {
                            LockId = lck.LockId,
                            AccessorId = accessorId,
                        });
                }
            }
            _logger.LogInformation($"Locks for accessor with AccessorId: {accessorId}, updated.");
            return Ok(await _locksAccessorsRepository.AddRangeAsync(itemsToUpdate));
        }

        //Patch Lock's Accessor
        [Authorize(Roles = UserRole.Admin)]
        [HttpPatch("[action]/{lockId}")]
        public async Task<ActionResult<LockAccessor>> UpdateLockAccessors(string lockId, [FromBody] List<string> accessorIds)
        {
            await _locksRepository.GetLockByIdAsync(lockId);

            var accessors = new List<ApplicationUser>();
            foreach (var accessorId in accessorIds)
            {
                var accessor = await _accessorsRepository.GetAccessorByIdAsync(accessorId);
                accessors.Add(accessor);
            }

            var itemsToDelete = await _locksAccessorsRepository.GetByLockIdAsync(lockId);
            if (itemsToDelete != null && itemsToDelete.Any())
            {
                await _locksAccessorsRepository.DeleteRangeAsync(itemsToDelete);
            }

            var itemsToUpdate = new List<LockAccessor>();
            if (accessors.Any())
            {
                foreach (var accessor in accessors)
                {
                    itemsToUpdate.Add(
                        new LockAccessor
                        {
                            LockId = new Guid(lockId),
                            AccessorId = accessor.Id,
                        });
                }
            }
            _logger.LogInformation($"Accessors for Lock with LockId: {lockId}, updated.");
            return Ok(await _locksAccessorsRepository.AddRangeAsync(itemsToUpdate));
        }
    }
}

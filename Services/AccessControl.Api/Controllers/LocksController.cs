using AccessControl.Api.Models.Entity;
using AccessControl.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Lib.Helpers;
using Shared.Lib.Models;

namespace AccessControl.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(Roles = $"{UserRole.Accessor}, {UserRole.Admin}")]
    [Route("api/[controller]")]
    [ApiController]
    public class LocksController : ControllerBase
    {
        private readonly ILocksAccessorsRepository _locksAccessorsRepository;
        private readonly ILocksRepository _locksRepository;
        private readonly IAccessorsRepository _accessorsRepository;
        private readonly IAccessesRepository _accessesRepository;
        private readonly ILogger<LocksController> _logger;

        public LocksController(ILocksAccessorsRepository locksAccessorsRepository, ILocksRepository locksRepository, IAccessorsRepository accessorsRepository, IAccessesRepository accessesRepository, ILogger<LocksController> logger)
        {
            _locksAccessorsRepository = locksAccessorsRepository ?? throw new ArgumentNullException(nameof(locksAccessorsRepository));
            _locksRepository = locksRepository ?? throw new ArgumentNullException(nameof(locksRepository));
            _accessorsRepository = accessorsRepository ?? throw new ArgumentNullException(nameof(accessorsRepository));
            _accessesRepository = accessesRepository ?? throw new ArgumentNullException(nameof(accessesRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //Put Lock
        [HttpPut("{accessorId}/{lockId}/{accessType}")]
        public async Task<ActionResult<Lock>> UpdateLock(string accessorId, string lockId, int accessType)
        {
            var accessor = await _accessorsRepository.GetAccessorByIdAsync(accessorId);

            var lck = await _locksRepository.GetLockByIdAsync(lockId);

            var requestedLockStatus = Helper.getLockStatus(accessType);
            if (requestedLockStatus == LockStatus.Undefined)
            {
                _logger.LogError($"AccessType with value: {accessType}, undefined.");
                throw new ApplicationException($"AccessType with value: {accessType}, undefined.");
            }

            var reason = "";
            var access = new Access()
            {
                AccessorId = accessorId,
                LockId = new Guid(lockId),
                Accesstype = requestedLockStatus,
            };

            var accessorLocks = await _locksAccessorsRepository.GetAccessorLocksAsync(accessorId);
            if (accessorLocks == null || !accessorLocks.Any())
            {
                reason = "You are not allowed to open any locks";
                access.Reason = reason;
                await _accessesRepository.AddAsync(access);
                return Unauthorized($"Unauthorized! {reason}");
            }

            var accessorLock = accessorLocks.FirstOrDefault(l => l.LockId.ToString().Equals(lockId), null);
            if (accessorLock == null)
            {
                reason = $"You are not allowed to open {lck.Name}";
                access.Reason = reason;
                await _accessesRepository.AddAsync(access);
                return Unauthorized($"Unauthorized! {reason}");
            }

            if (!accessorLock.AllowUnlocking && requestedLockStatus.Equals(LockStatus.Unlock))
            {
                reason = $"{lck.Name} is not Unlockable";
                access.Reason = reason;
                await _accessesRepository.AddAsync(access);
                return Unauthorized($"Unauthorized! {reason}");
            }

            lck.IsLocked = requestedLockStatus.Equals(LockStatus.Unlock) ? false : true;

            await _locksRepository.UpdateAsync(lck);

            access.IsSuccessful = true;
            await _accessesRepository.AddAsync(access);
            _logger.LogInformation($"Lock with LockId: {lck.LockId} is successfully updated.");

            return Ok(lck);
        }
    }
}

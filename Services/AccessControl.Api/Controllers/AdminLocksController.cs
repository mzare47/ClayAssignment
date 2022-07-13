using AccessControl.Api.Models.Entity;
using AccessControl.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Lib.Models;

namespace AccessControl.Api.Controllers
{
    [Authorize(Roles = UserRole.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminLocksController : ControllerBase
    {
        private readonly ILocksRepository _locksRepository;
        private readonly ILogger<AdminLocksController> _logger;

        public AdminLocksController(ILocksRepository locksRepository, ILogger<AdminLocksController> logger)
        {
            _locksRepository = locksRepository ?? throw new ArgumentNullException(nameof(locksRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //Get Locks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lock>>> GetAllLocks()
        {
            var locks = await _locksRepository.GetAllAsync();
            _logger.LogInformation($"All locks, retrieved.");
            return Ok(locks);
        }

        //Get Lock By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Lock>> GetLockById(string id)
        {
            var lck = await _locksRepository.GetLockByIdAsync(id);
            _logger.LogInformation($"Lock with LockId: {id}, retrieved.");
            return Ok(lck);
        }

        //Post Lock
        [HttpPost]
        public async Task<ActionResult<Lock>> CreateLock([FromBody] Lock lck)
        {
            await _locksRepository.AddAsync(lck);
            _logger.LogInformation($"Lock with LockId: {lck.LockId}, created.");
            return CreatedAtRoute("", new { id = lck.LockId.ToString() }, lck);
        }

        //Put Lock
        [HttpPut]
        public async Task<ActionResult<Lock>> UpdateLock([FromBody] Lock lck)
        {
            var lckToUpdate = await _locksRepository.GetLockByIdAsync(lck.LockId.ToString());
            lck.CreatedBy = lckToUpdate.CreatedBy;
            lck.CreatedDate = lckToUpdate.CreatedDate;
            await _locksRepository.UpdateAsync(lck);
            _logger.LogInformation($"Lock with LockId: {lck.LockId} is successfully updated.");
            return Ok(lck);
        }

        //Delete Lock
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lock>> DeleteLock(string id)
        {
            var lckToDelete = await _locksRepository.GetByIdAsync(new Guid(id));
            await _locksRepository.DeleteAsync(lckToDelete);
            _logger.LogInformation($"Lock with LockId: {id} is successfully deleted.");
            return Ok(lckToDelete);
        }

    }
}

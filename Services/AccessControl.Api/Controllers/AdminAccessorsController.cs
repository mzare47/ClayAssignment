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
    public class AdminAccessorsController : ControllerBase
    {
        private readonly IAccessorsRepository _accessorsRepository;
        private readonly ILogger<AdminAccessorsController> _logger;

        public AdminAccessorsController(IAccessorsRepository accessorsRepository, ILogger<AdminAccessorsController> logger)
        {
            _accessorsRepository = accessorsRepository ?? throw new ArgumentNullException(nameof(accessorsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //Get All Accessors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllAccessors()
        {
            var accessors = await _accessorsRepository.GetAllAccessors();
            _logger.LogInformation($"All accessors, retrieved.");
            return Ok(accessors);
        }

        //Get Accessor By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Lock>> GetAccessorById(string id)
        {
            var accessor = await _accessorsRepository.GetAccessorByIdAsync(id);
            _logger.LogInformation($"Accessor with AccessorId: {id}, retrieved.");
            return Ok(accessor);
        }

        //Delete Accessor
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lock>> DeleteAccessor(string id)
        {
            var accessorToDelete = await _accessorsRepository.GetAccessorByIdAsync(id);
            await _accessorsRepository.DeleteAccessorAsync(accessorToDelete);
            _logger.LogInformation($"Accessor with AccessorId: {id}, is successfully deleted.");
            return Ok(accessorToDelete);
        }
    }
}

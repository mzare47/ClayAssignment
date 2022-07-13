using Microsoft.AspNetCore.Mvc;
using WebApp.Admin.Models;
using WebApp.Admin.Services;

namespace WebApp.Admin.Controllers
{
    public class AccessorController : Controller
    {
        private readonly ILogger<AccessorController> _logger;
        private readonly IAccessControlService _accessControlService;

        public AccessorController(ILogger<AccessorController> logger, IAccessControlService accessControlService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accessControlService = accessControlService ?? throw new ArgumentNullException(nameof(accessControlService));
        }

        public async Task<IActionResult> Index()
        {
            return View(await _accessControlService.GetAllAccessors());
        }

        public async Task<IActionResult> Delete(string accessorId)
        {
            await _accessControlService.DeleteAccessor(accessorId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccessorToAddDto accessor)
        {
            if (ModelState.IsValid)
            {
                await _accessControlService.CreateAccessor(accessor);
                return RedirectToAction(nameof(Index));
            }
            return View(accessor);
        }

        public async Task<IActionResult> AccessManagement(string accessorId)
        {
            var accessor = await _accessControlService.GetAccessorById(accessorId);
            var allowedlocks = await _accessControlService.GetAccessorLocks(accessorId);
            var accessMgmt = new AccessorAccessMgmtDto()
            {
                Accessor = accessor,
                AllowedLockIds = allowedlocks.Select(a => a.LockId.ToString()).ToList(),
            };
            ViewBag.allLocks = await _accessControlService.GetAllLocks();
            return View(accessMgmt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccessManagement(AccessorAccessMgmtDto accessMgmt, List<string> selectedLockIds)
        {
            if (ModelState.IsValid)
            {
                await _accessControlService.UpdateAccessorLocks(accessMgmt.Accessor.Id, selectedLockIds);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.allLocks = await _accessControlService.GetAllLocks();
            return View(accessMgmt);
        }
    }
}

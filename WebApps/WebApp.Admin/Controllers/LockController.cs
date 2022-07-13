using Microsoft.AspNetCore.Mvc;
using WebApp.Admin.Models;
using WebApp.Admin.Services;

namespace WebApp.Admin.Controllers
{
    public class LockController : Controller
    {
        private readonly ILogger<LockController> _logger;
        private readonly IAccessControlService _accessControlService;

        public LockController(ILogger<LockController> logger, IAccessControlService accessControlService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accessControlService = accessControlService ?? throw new ArgumentNullException(nameof(accessControlService));
        }

        public async Task<IActionResult> Index()
        {
            return View(await _accessControlService.GetAllLocks());
        }

        public async Task<IActionResult> Delete(string lockId)
        {
            await _accessControlService.DeleteLock(lockId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LockToAddDto lck)
        {
            if (ModelState.IsValid)
            {
                await _accessControlService.CreateLock(lck);
                return RedirectToAction(nameof(Index));
            }
            return View(lck);
        }

        public async Task<IActionResult> Edit(string lockId)
        {
            var lck = await _accessControlService.GetLockById(lockId);
            return View(lck);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LockDto lck)
        {
            if (ModelState.IsValid)
            {
                await _accessControlService.EditLock(lck);
                return RedirectToAction(nameof(Index));
            }
            return View(lck);
        }

        public async Task<IActionResult> AccessManagement(string lockId)
        {
            var lck = await _accessControlService.GetLockById(lockId);
            var allowedAccessors = await _accessControlService.GetLockAccessors(lockId);
            var accessMgmt = new LockAccessMgmtDto()
            {
                Lock = lck,
                AllowedAccessorIds = allowedAccessors.Select(a => a.Id).ToList(),
            };
            ViewBag.allAccessors = await _accessControlService.GetAllAccessors();
            return View(accessMgmt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccessManagement(LockAccessMgmtDto accessMgmt, List<string> selectedAccessorIds)
        {
            if (ModelState.IsValid)
            {
                await _accessControlService.UpdateLockAccessors(accessMgmt.Lock.LockId, selectedAccessorIds);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.allAccessors = await _accessControlService.GetAllAccessors();
            return View(accessMgmt);
        }
    }
}

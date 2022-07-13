using Microsoft.AspNetCore.Mvc;
using WebApp.Admin.Services;

namespace WebApp.Admin.Controllers
{
    public class AccessController : Controller
    {
        private readonly ILogger<AccessController> _logger;
        private readonly IAccessControlService _accessControlService;

        public AccessController(ILogger<AccessController> logger, IAccessControlService accessControlService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accessControlService = accessControlService ?? throw new ArgumentNullException(nameof(accessControlService));
        }
        public async Task<IActionResult> Index()
        {
            return View(await _accessControlService.GetAllAccesses());
        }
    }
}

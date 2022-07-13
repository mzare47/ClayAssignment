using Microsoft.AspNetCore.Mvc;
using Shared.Lib.Helpers;
using System.Diagnostics;
using System.Security.Claims;
using WebApp.Accessor.Models;
using WebApp.Accessor.Services;

namespace WebApp.Accessor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccessControlService _accessControlService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IAccessControlService accessControlService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accessControlService = accessControlService ?? throw new ArgumentNullException(nameof(accessControlService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<IActionResult> Index()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("token");
            ViewBag.token = token;
            var userId = Helper.getUserIdFromToken(token);
            ViewBag.userId = userId;
            return View(await _accessControlService.GetAccessorLocks(userId));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
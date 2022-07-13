using Microsoft.AspNetCore.Mvc;
using WebApp.Admin.Models;
using WebApp.Admin.Services;

namespace WebApp.Admin.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAccessControlService _accessControlService;

        public AuthController(ILogger<AuthController> logger, IAccessControlService accessControlService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accessControlService = accessControlService ?? throw new ArgumentNullException(nameof(accessControlService));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var token = await _accessControlService.GetToken(model);
                HttpContext.Session.SetString("token", token.Token);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction(nameof(Login));
        }
    }
}

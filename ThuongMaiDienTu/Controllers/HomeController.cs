using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else if (roles.Contains("Customer"))
                {
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Guest/Favourite/{id}")]
        [HttpPost]
        public async Task<IActionResult> Favourite(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    success = false,
                    redirect = Url.Action("Login", "Account"),
                    message = ""
                });
            }

            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Customer"))
            {
                // Trả về URL để frontend gọi tiếp
                return Json(new
                {
                    success = true,
                    forwardTo = Url.Action("Create", "Favourite", new { id = id, area = "Customer" }) // hoặc "/Customer/Favourite/Create/{id}"
                });
            }

            return Json(new
            {
                success = false,
                message = "Bạn không có quyền thực hiện hành động này."
            });
        }

    }
}

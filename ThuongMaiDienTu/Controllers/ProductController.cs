using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThuongMaiDienTu.Models;
using ThuongMaiDienTu.Services;

namespace ThuongMaiDienTu.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _product;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(IProductService product, UserManager<AppUser> userManager)
        {
            _product = product;
            _userManager = userManager;
        }

        [Route("/Product/List")]
        public async Task<IActionResult> List()
        {
            var list = await _product.List();
            return Json(new {Data = list });
        }
        [Route("/Product/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Customer"))
                {
                    return RedirectToAction("Details", "Product", new { area = "Customer", id = id });
                }
            }
            var item = await _product.Detail(id);
            var idLastestLaunch = item.Launches.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            ViewBag.Registration = await _product.CountRegistration(id, idLastestLaunch);
            ViewBag.Quantity = await _product.TotalProductOfLaunch(idLastestLaunch);
            return View(item);
        }
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThuongMaiDienTu.Areas.Customer.Models;
using ThuongMaiDienTu.Areas.Customer.Services;

namespace ThuongMaiDienTu.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFeaturedProductService _featuredProductService;

        public HomeController(ILogger<HomeController> logger, IFeaturedProductService featuredProductService)
        {
            _logger = logger;
            _featuredProductService = featuredProductService;
        }
        
        [Route("customer")]
        public async Task<IActionResult> Index()
        {
            var featuredProduct = await _featuredProductService.GetFeaturedProduct();
            return View(featuredProduct);
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



    }
}

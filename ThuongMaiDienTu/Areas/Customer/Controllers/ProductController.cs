using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThuongMaiDienTu.Models;
using ThuongMaiDienTu.Services;

namespace ThuongMaiDienTu.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class ProductController : Controller
    {
        private readonly IProductService _product;

        public ProductController(IProductService product)
        {
            _product = product;
        }

        [Route("Customer/Product/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _product.Detail(id);
            var idLastestLaunch = item.Launches.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            ViewBag.Registration = await _product.CountRegistration(id, idLastestLaunch);
            ViewBag.Quantity = await _product.TotalProductOfLaunch(idLastestLaunch);
            return View(item);
        }
    }
}

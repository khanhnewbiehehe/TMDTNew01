using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThuongMaiDienTu.Services;

namespace ThuongMaiDienTu.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _category;

        public CategoryController(ICategoryService category)
        {
            _category = category;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("/Category/List")]
        public async Task<IActionResult> List()
        {
            var list = await _category.List();
            return Json(new {Data = list});
        }
    }
}

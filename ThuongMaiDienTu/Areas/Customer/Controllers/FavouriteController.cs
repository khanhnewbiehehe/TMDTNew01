using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThuongMaiDienTu.Areas.Customer.Services;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class FavouriteController : Controller
    {
        private readonly IFavouriteCustomerService _favourite;
        private readonly UserManager<AppUser> _userManager;

        public FavouriteController(IFavouriteCustomerService favourite, UserManager<AppUser> userManager)
        {
            _favourite = favourite;
            _userManager = userManager;
        }

        [Route("Customer/Favourite")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var favourite = await _favourite.Find(user.Id);
            if(favourite == null)
            {
                await _favourite.Init(user.Id);
            }
            var list = await _favourite.List(user.Id);
            return View(list);
        }
        [Route("Customer/Favourite/Delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _favourite.Delete(id, user.Id);
            return Json(result);
        }

        [Route("Customer/Favourite/Create/{id}")]
        [HttpPost]
        public async Task<IActionResult> Create(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var favourite = await _favourite.Find(user.Id);
            if (favourite == null)
            {
                await _favourite.Init(user.Id);
            }

            var result = await _favourite.Create(id, user.Id);

            if (result)
            {
                return Json(new
                {
                    success = true,
                    message = "Sản phẩm đã được thêm vào danh sách yêu thích."
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Không thể thêm sản phẩm vào danh sách yêu thích."
                });
            }
        }

    }
}

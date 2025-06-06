using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThuongMaiDienTu.Models;
using ThuongMaiDienTu.ViewModels;

namespace ThuongMaiDienTu.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Route("Customer/Account")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user); 
        }
        [Route("Customer/Account/Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {

            var user = await _userManager.GetUserAsync(User);
            var info = new RegisterVM();
            info.FullName = user.FullName;
            info.PhoneNumber = user.PhoneNumber;
            info.Email = user.Email;
            info.AvatarUrl = user.AvatarUrl;
            return View(info);
        }

        [HttpPost]
        [Route("Customer/Account/Edit/{id}")]
        public async Task<IActionResult> Edit(string id, RegisterVM model, IFormFile AvatarFile = null)
        {
            if (!ModelState.IsValid)
            {
                // Nếu dữ liệu không hợp lệ, trả về view cùng dữ liệu
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin user
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;

            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var originalFileName = Path.GetFileName(AvatarFile.FileName);
                var timestamp = DateTime.Now.Ticks.ToString();
                var fileName = timestamp + "_" + originalFileName;  // Thêm tiền tố ticks trước tên file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AvatarFile.CopyToAsync(stream);
                }

                user.AvatarUrl = "/Uploads/" + fileName;
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Cập nhật thành công!";
                return RedirectToAction("Index", "Account", new { area = "Customer" });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

    }
}

using ThuongMaiDienTu.Models;
using ThuongMaiDienTu.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ThuongMaiDienTu.Controllers;

public class AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) : Controller
{
    [Route("/Account/Login")]
    public async Task<IActionResult> Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await userManager.GetUserAsync(User);
            var roles = await userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else
            {
                if (roles.Contains("Customer"))
                {
                    return RedirectToAction("index", "Home", new { area = "Customer" });
                }
                else
                    return RedirectToAction("index", "home");
            }
        }
        return View();
    }

    [HttpPost]
    [Route("/Account/Login")]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (ModelState.IsValid)
        {
            var username = model.Email.Trim().ToUpperInvariant();
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                //login
                var result = await signInManager.PasswordSignInAsync(username!, model.Password!, model.RememberMe, false);

                if (result.Succeeded)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    }
                    else
                    {
                        if (roles.Contains("Customer"))
                        {
                            return RedirectToAction("index", "Home", new { area = "Customer" });
                        }
                        else
                            return RedirectToAction("index", "home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập thất bại!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại!");
            }
            return View(model);
        }
        return View(model);
    }
    [Route("/Account/Register")]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [Route("/Account/Register")]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        if (ModelState.IsValid)
        {
            AppUser user = new AppUser
            {
                FullName = model.FullName,
                UserName = model.Email.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber.Trim(),
                NormalizedEmail = model.Email.Trim().ToUpperInvariant()
            };

            var result = await userManager.CreateAsync(user, model.Password!);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Customer");

                await signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(model);
    }

    [Route("/dang-xuat")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index","Home");
    }
    [HttpGet]
    [AllowAnonymous]
    [Route("opps")]
    public IActionResult AccessDenied()
    {
        return View();
    }

}

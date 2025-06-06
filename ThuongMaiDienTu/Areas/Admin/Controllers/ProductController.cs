using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThuongMaiDienTu.Data;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                                         .Include(p => p.Category)
                                         .Include(p => p.Images)
                                         .Include(p => p.Launches)
                                         .ToListAsync();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                product.CreateAt = DateTime.Now;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                if (images != null)
                {
                    foreach (var img in images)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                        var path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await img.CopyToAsync(stream);
                        }
                        _context.ProductImages.Add(new ProductImage
                        {
                            ProductId = product.Id,
                            Url = "/assets/images/" + fileName,
                            Description = img.FileName
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                TempData["SuccessMessage"] = "Thêm " + (product?.Name ?? "sản phẩm") + " thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, List<IFormFile> newImages, List<int> deleteImageIds)
        {
            if (ModelState.IsValid)
            {
                _context.Update(product);

                if (deleteImageIds != null)
                {
                    var imagesToDelete = _context.ProductImages.Where(i => deleteImageIds.Contains(i.Id));
                    _context.ProductImages.RemoveRange(imagesToDelete);
                }

                if (newImages != null)
                {
                    foreach (var img in newImages)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                        var path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await img.CopyToAsync(stream);
                        }
                        _context.ProductImages.Add(new ProductImage
                        {
                            ProductId = product.Id,
                            Url = "/assets/images/" + fileName,
                            Description = img.FileName
                        });
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật " + (product?.Name ?? "sản phẩm") + " thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
            try
            {
                if (product != null)
                {
                    _context.ProductImages.RemoveRange(product.Images);
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
                TempData["SuccessMessage"] = "Xóa " + (product?.Name ?? "sản phẩm") + " thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Kiểm tra có lỗi khóa ngoại không
                if (ex.InnerException?.Message.Contains("REFERENCE constraint") == true)
                {
                    TempData["DeleteError"] = "Không thể xóa " + (product?.Name ?? "sản phẩm") + " này vì dữ liệu đang được sử dụng ở nơi khác. Vui lòng kiểm tra và xóa dữ liệu liên quan trước.";
                }
                else
                {
                    TempData["DeleteError"] = "Đã xảy ra lỗi khi xóa dữ liệu.";
                }

                return RedirectToAction("Index");
            }
        }
    }
}

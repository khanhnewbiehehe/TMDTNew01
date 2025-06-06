using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThuongMaiDienTu.Data;
using ThuongMaiDienTu.Areas.Admin.ViewModels;
using ThuongMaiDienTu.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThuongMaiDienTu.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ProductLaunchController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductLaunchController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var launches = _context.ProductLaunchs
                .Include(pl => pl.Product)
                .Select(pl => new ProductLaunchViewModel
                {
                    Id = pl.Id,
                    Name = pl.Name,
                    Description = pl.Description,
                    DateStart = pl.DateStart,
                    DateEnd = pl.DateEnd,
                    ProductName = pl.Product.Name,
                    Status = DateTime.Now < pl.DateStart
                        ? "Sắp mở"
                        : (DateTime.Now <= pl.DateEnd ? "Đang mở" : "Đã đóng")
                })
                .ToList();

            return View(launches);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductLaunchCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(model);
            }

            var launch = new ProductLaunch
            {
                Name = model.LaunchName,
                Description = model.Description,
                DateStart = model.DateStart,
                DateEnd = model.DateEnd,
                ProductId = (int)model.ProductId,
                Types = new List<ProductType>()
            };

            foreach (var pt in model.ProductTypes)
            {
                var product = new ProductType
                {
                    Name = pt.Name,
                    Quantity = pt.Quantity,
                    MaxPrice = pt.MaxPrice,
                    Prices = pt.PriceItems.Select(p => new PriceItem
                    {
                        Number = p.Quantity,
                        Price = p.Price
                    }).ToList()
                };

                if (pt.ImageFile != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(pt.ImageFile.FileName);
                    var path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await pt.ImageFile.CopyToAsync(stream);
                    }
                    product.ImageUrl = "/assets/images/" + fileName;
                }

                launch.Types.Add(product);
            }

            _context.ProductLaunchs.Add(launch);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm " + (launch?.Name ?? "đợt mở bán") + " thành công!";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var launch = await _context.ProductLaunchs
                .Include(p => p.Types)
                    .ThenInclude(t => t.Prices)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (launch == null) return NotFound();

            var viewModel = new ProductLaunchCreateViewModel
            {
                Id = launch.Id,
                LaunchName = launch.Name,
                Description = launch.Description,
                DateStart = launch.DateStart,
                DateEnd = launch.DateEnd,
                ProductId = launch.ProductId,
                ProductTypes = launch.Types.Select(t => new ProductTypeViewModel
                {
                    Name = t.Name,
                    Quantity = t.Quantity,
                    MaxPrice = (long)t.MaxPrice,
                    ImageUrl = t.ImageUrl,
                    PriceItems = t.Prices.Select(p => new PriceItemViewModel
                    {
                        Quantity = p.Number,
                        Price = (long)p.Price
                    }).ToList()
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var launch = await _context.ProductLaunchs
                .Include(p => p.Types)
                    .ThenInclude(t => t.Prices)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (launch == null)
                return NotFound();

            var viewModel = new ProductLaunchCreateViewModel
            {
                Id = launch.Id,
                LaunchName = launch.Name,
                Description = launch.Description,
                DateStart = launch.DateStart,
                DateEnd = launch.DateEnd,
                ProductId = launch.ProductId,
                ProductTypes = launch.Types.Select(t => new ProductTypeViewModel
                {
                    Name = t.Name,
                    Quantity = t.Quantity,
                    MaxPrice = (long)t.MaxPrice,
                    ImageUrl = t.ImageUrl,
                    PriceItems = t.Prices.Select(p => new PriceItemViewModel
                    {
                        Quantity = p.Number,
                        Price = (long)p.Price
                    }).ToList()
                }).ToList()
            };

            ViewBag.Products = new SelectList(_context.Products, "Id", "Name", launch.ProductId);
            return View(viewModel);
        }


        [HttpPost]
public async Task<IActionResult> Edit(ProductLaunchCreateViewModel model)
{
    if (!ModelState.IsValid)
    {
        ViewBag.Products = new SelectList(_context.Products, "Id", "Name", model.ProductId);
        return View(model);
    }

    var launch = await _context.ProductLaunchs
        .Include(p => p.Types)
            .ThenInclude(t => t.Prices)
        .FirstOrDefaultAsync(p => p.Id == model.Id);

    if (launch == null)
        return NotFound();

    // Cập nhật thông tin chung
    launch.Name = model.LaunchName;
    launch.Description = model.Description;
    launch.DateStart = model.DateStart;
    launch.DateEnd = model.DateEnd;
    launch.ProductId = model.ProductId;

    // Xóa dữ liệu cũ
    _context.PriceItems.RemoveRange(launch.Types.SelectMany(t => t.Prices));
    _context.ProductTypes.RemoveRange(launch.Types);
    launch.Types.Clear();

    // Thêm lại dữ liệu mới
    foreach (var pt in model.ProductTypes)
    {
        var product = new ProductType
        {
            Name = pt.Name,
            Quantity = pt.Quantity,
            MaxPrice = pt.MaxPrice,
            Prices = pt.PriceItems.Select(p => new PriceItem
            {
                Number = p.Quantity,
                Price = p.Price
            }).ToList()
        };

        if (pt.ImageFile != null)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(pt.ImageFile.FileName);
            var path = Path.Combine(_env.WebRootPath, "assets/images", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await pt.ImageFile.CopyToAsync(stream);
            }
            product.ImageUrl = "/assets/images/" + fileName;
        }
        else
        {
            product.ImageUrl = pt.ImageUrl; // giữ lại ảnh cũ nếu không upload ảnh mới
        }

        launch.Types.Add(product);
    }

    await _context.SaveChangesAsync();
    TempData["SuccessMessage"] = "Cập nhật thành công!";
    return RedirectToAction("Index");
}


        public async Task<IActionResult> Delete(int id)
        {
            var launch = await _context.ProductLaunchs
                .Include(x => x.Types)
                    .ThenInclude(x => x.Prices)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (launch == null)
                return NotFound();
            try
            {
                _context.PriceItems.RemoveRange(launch.Types.SelectMany(t => t.Prices));
                _context.ProductTypes.RemoveRange(launch.Types);
                _context.ProductLaunchs.Remove(launch);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa " + (launch?.Name ?? "đợt mở bán") + " thành công!";
                return RedirectToAction("Index");

            }
            catch (DbUpdateException ex)
            {
                // Kiểm tra có lỗi khóa ngoại không  
                if (ex.InnerException?.Message.Contains("REFERENCE constraint") == true)
                {
                    TempData["DeleteError"] = "Không thể xóa " + (launch?.Name ?? "đợt mở bán") + " này vì dữ liệu đang được sử dụng ở nơi khác. Vui lòng kiểm tra và xóa dữ liệu liên quan trước.";
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

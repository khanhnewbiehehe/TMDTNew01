using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThuongMaiDienTu.ViewModels;
using ThuongMaiDienTu.Services;
using System.Linq;
using System;

namespace ThuongMaiDienTu.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class SearchController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public SearchController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }
        public async Task<IActionResult> Index(string searchTerm = "", int categoryId = 0, decimal minPrice = 0, decimal maxPrice = decimal.MaxValue, bool LaunchGoing = true, bool LaunchFull = true, bool LaunchEnded = true, int page = 1, int pageSize = 12)
        {
            var categories = await _categoryService.List();
            var allProducts = await _productService.List();

            // Apply filters
            var filteredProducts = allProducts;

            // Filter by search term
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredProducts = filteredProducts.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filter by category
            if (categoryId > 0)
            {
                filteredProducts = filteredProducts.Where(p => p.CategoryId == categoryId).ToList();
            }              // Filter by price
            if (maxPrice == 0 || maxPrice == decimal.MinValue) maxPrice = decimal.MaxValue; // Handle case where maxPrice is not set
            filteredProducts = filteredProducts.Where(p => p.Min >= minPrice && (maxPrice == decimal.MaxValue || p.Max <= maxPrice)).ToList();

            // Filter by status
            DateTime now = DateTime.Now;
            if (!LaunchGoing || !LaunchFull || !LaunchEnded)
            {
                filteredProducts = filteredProducts.Where(p =>
                    // Show ongoing products (not ended and not full)
                    (LaunchGoing && p.End > now && p.Registration < p.Quantity) ||
                    // Show full products (not ended but full)
                    (LaunchFull && p.End > now && p.Registration >= p.Quantity) ||
                    // Show ended products
                    (LaunchEnded && p.End <= now)
                ).ToList();
            }

            var viewModel = new SearchViewModel
            {
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                LaunchGoing = LaunchGoing,
                LaunchFull = LaunchFull,
                LaunchEnded = LaunchEnded,
                Page = page,
                PageSize = pageSize,
                Categories = categories,
                Products = filteredProducts
            };

            return View(viewModel);
        }
        [HttpGet]
        [AllowAnonymous] // Allow anonymous access for AJAX calls
        public async Task<IActionResult> GetFilteredProducts(string searchTerm = "", int categoryId = 0, decimal minPrice = 0, decimal maxPrice = decimal.MaxValue, bool LaunchGoing = true, bool LaunchFull = true, bool LaunchEnded = true, int page = 1, int pageSize = 12)
        {
            var allProducts = await _productService.List();

            // Apply filters
            var filteredProducts = allProducts;

            // Filter by search term
            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredProducts = filteredProducts.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filter by category
            if (categoryId > 0)
            {
                filteredProducts = filteredProducts.Where(p => p.CategoryId == categoryId).ToList();
            }
            // Filter by price range
            if (maxPrice == 0 || maxPrice == decimal.MinValue) maxPrice = decimal.MaxValue; // Handle case where maxPrice is not set
            filteredProducts = filteredProducts.Where(p => p.Min >= minPrice && (maxPrice == decimal.MaxValue || p.Max <= maxPrice)).ToList();

            // Filter by status
            DateTime now = DateTime.Now;
            if (!LaunchGoing || !LaunchFull || !LaunchEnded)
            {
                filteredProducts = filteredProducts.Where(p =>
                    // Show ongoing products (not ended and not full)
                    (LaunchGoing && p.End > now && p.Registration < p.Quantity) ||
                    // Show full products (not ended but full)
                    (LaunchFull && p.End > now && p.Registration >= p.Quantity) ||
                    // Show ended products
                    (LaunchEnded && p.End <= now)
                ).ToList();
            }

            // Apply pagination
            var paginatedProducts = filteredProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Json(new { success = true, data = paginatedProducts, total = filteredProducts.Count() });
        }
    }
}
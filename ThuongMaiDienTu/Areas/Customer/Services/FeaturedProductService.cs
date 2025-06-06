using Microsoft.EntityFrameworkCore;
using ThuongMaiDienTu.Data;
using ThuongMaiDienTu.Services;
using ThuongMaiDienTu.ViewModels;

namespace ThuongMaiDienTu.Areas.Customer.Services
{
    public class FeaturedProductService : IFeaturedProductService
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;

        public FeaturedProductService(AppDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public async Task<ProductCardVM> GetFeaturedProduct()
        {
            // Get the product with the highest productLaunchId
            var latestLaunch = await _context.ProductLaunchs
                .Include(l => l.Product)
                    .ThenInclude(p => p.Images)
                .Include(l => l.Types)
                    .ThenInclude(t => t.Prices)
                .OrderByDescending(l => l.Id)
                .FirstOrDefaultAsync();

            if (latestLaunch == null)
                return null;

            var product = latestLaunch.Product;
            // Create ProductCardVM with the data
            var featuredProduct = new ProductCardVM
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                ImageUrl = product.Images?.FirstOrDefault()?.Url ?? "default.jpg",
                End = latestLaunch.DateEnd,
                Description = product.Description // Add description from product model
            }
            ;

            // Get min and max prices
            if (latestLaunch.Types != null && latestLaunch.Types.Any() &&
                latestLaunch.Types.FirstOrDefault()?.Prices != null &&
                latestLaunch.Types.FirstOrDefault().Prices.Any())
            {
                var prices = latestLaunch.Types.SelectMany(t => t.Prices).Select(p => p.Price);
                featuredProduct.Min = prices.Min();
                featuredProduct.Max = prices.Max();
            }

            // Get registration count and total quantity
            featuredProduct.Registration = await _productService.CountRegistration(product.Id, latestLaunch.Id);
            featuredProduct.Quantity = await _productService.TotalProductOfLaunch(latestLaunch.Id);

            return featuredProduct;
        }
    }
}

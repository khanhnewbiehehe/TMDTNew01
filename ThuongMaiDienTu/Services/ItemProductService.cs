using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using ThuongMaiDienTu.Data;
using ThuongMaiDienTu.Models;
using ThuongMaiDienTu.ViewModels;

namespace ThuongMaiDienTu.Services
{
    public class ItemProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ItemProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountRegistration(int id, int idLaunch)
        {
            int Registration = 0;
            var listInvoice = await _context.InvoicesItem.Include(x => x.Invoice)
                                                         .Include(x => x.ProductType)
                                                            .ThenInclude(x => x.ProductLaunch)
                                                                .ThenInclude(x => x.Product)
                                                     .Where(x => x.ProductType.ProductLaunch.Product.Id == id && x.ProductType.ProductLaunch.Id == idLaunch)
                                                     .ToListAsync();
            
            foreach (var item in listInvoice)
            {
                if(item.Invoice.Status != -1 && item.Invoice.Status != -100)
                {
                    Registration += item.Quantity;
                }
            }
            return Registration;

        }

        public async Task<Product> Detail(int id)
        {
            var item = await _context.Products.Include(x => x.Images)
                                              .Include(x => x.Launches)
                                                .ThenInclude(x => x.Types)
                                                    .ThenInclude(x => x.Prices)
                                              .FirstOrDefaultAsync(x => x.Id == id);
            return item;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
        }

        public async Task<IEnumerable<ProductCardVM>> List()
        {

            var listProduct = await _context.Products.Include(x => x.Images)
                                              .Include(x => x.Launches)
                                                .ThenInclude(x => x.Types)
                                                    .ThenInclude(x => x.Prices)
                                              .ToListAsync();
            var result = new List<ProductCardVM>();
            foreach (var item in listProduct)
            {
                var newProduct = new ProductCardVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    ImageUrl = item.Images?.FirstOrDefault()?.Url ?? "default.jpg" // hoặc null tùy bạn
                };

                var latestLaunch = item.Launches?.OrderByDescending(x => x.Id).FirstOrDefault();
                if (latestLaunch != null)
                {
                    newProduct.End = latestLaunch.DateEnd;

                    var firstType = latestLaunch.Types?.FirstOrDefault();
                    if (firstType != null && firstType.Prices != null && firstType.Prices.Any())
                    {
                        newProduct.Min = firstType.Prices.Min(x => x.Price);
                        newProduct.Max = firstType.Prices.Max(x => x.Price);
                    }
                }

                newProduct.Registration = await CountRegistration(item.Id, latestLaunch.Id);
                newProduct.Quantity = await TotalProductOfLaunch(latestLaunch.Id);

                result.Add(newProduct);
            }



            return result;
        }

        public async Task<int> TotalProductOfLaunch(int idLaunch)
        {
            int total = 0;
            var launch = await _context.ProductLaunchs.Include(x => x.Types).FirstOrDefaultAsync(x => x.Id == idLaunch);

            total = launch.Types.Sum(x => x.Quantity);
            return total;
        }
    }
}

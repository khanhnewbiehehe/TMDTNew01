using Microsoft.EntityFrameworkCore;
using ThuongMaiDienTu.Data;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Areas.Customer.Services
{
    public class ItemProductTypeCustomerService : IProductTypeCustomerService
    {
        private readonly AppDbContext _context;

        public ItemProductTypeCustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> CurrentPrice(int id)
        {
            var item = await _context.ProductTypes.Include(x => x.ProductLaunch)
                                                  .Include(x => x.Prices)
                                                  .FirstOrDefaultAsync(x => x.Id == id);
            var launchId = item.ProductLaunch.Id;

            var launch = await _context.ProductLaunchs.Include(x => x.Types)
                                                        .ThenInclude(x => x.InvoiceItems)
                                                            .ThenInclude(x => x.Invoice)
                                                      .FirstOrDefaultAsync(x => x.Id == launchId);

            int soLuongHienTai = 0;

            foreach(var type in launch.Types)
            {
                soLuongHienTai += type.InvoiceItems.Where(x => x.Invoice.Status != -1 && x.Invoice.Status != -100).Sum(x => x.Quantity);
            }

            decimal result = item.MaxPrice;

            foreach(var price in item.Prices)
            {
                if(soLuongHienTai > price.Number)
                {
                    result = price.Price;
                }
            }

            return result;

        }

        public async Task<ProductType> Details(int id)
        {
            var item = await _context.ProductTypes.FindAsync(id);
            return item;
        }
    }
}

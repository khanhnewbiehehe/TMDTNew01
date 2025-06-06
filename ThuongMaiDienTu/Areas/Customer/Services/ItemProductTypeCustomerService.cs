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

        public async Task<ProductType> Details(int id)
        {
            var item = await _context.ProductTypes.FindAsync(id);
            return item;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ThuongMaiDienTu.Data;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Areas.Customer.Services
{
    public class ItemInvoiceCustomerService : IInvoiceCustomerService
    {
        private readonly AppDbContext _context;

        public ItemInvoiceCustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Invoice>> ListByCustomer(string id)
        {
            var list = await _context.Invoices.Include(x => x.InvoiceItems)
                                                .ThenInclude(x => x.ProductType)
                                                    .ThenInclude(x => x.ProductLaunch)
                                                        .ThenInclude(x => x.Product)
                                                            .ThenInclude(x => x.Images)
                                              .Where(x => x.UserId == id && x.Status != -100)
                                              .OrderByDescending(x => x.Id)
                                              .ToListAsync();
            return list;
        }

        public async Task<bool> Order(Invoice model)
        {
            try
            {
                _context.Invoices.Add(model);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

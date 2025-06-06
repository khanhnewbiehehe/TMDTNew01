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

        public Task<IEnumerable<Invoice>> ListByCustomer(string id)
        {
            throw new NotImplementedException();
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

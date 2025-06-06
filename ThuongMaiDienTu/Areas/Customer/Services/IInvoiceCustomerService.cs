using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Areas.Customer.Services
{
    public interface IInvoiceCustomerService
    {
        Task<bool> Order(Invoice model);
        Task<IEnumerable<Invoice>> ListByCustomer(string id);
        Task<Invoice> Details(int id);
        Task<bool> Cancel(int id);
    }
}

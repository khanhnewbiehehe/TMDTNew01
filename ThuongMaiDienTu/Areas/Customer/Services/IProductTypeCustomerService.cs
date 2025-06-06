using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Areas.Customer.Services
{
    public interface IProductTypeCustomerService
    {
        Task<ProductType> Details(int id);
    }
}

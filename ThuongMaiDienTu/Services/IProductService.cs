using ThuongMaiDienTu.Models;
using ThuongMaiDienTu.ViewModels;

namespace ThuongMaiDienTu.Services
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductCardVM>> List();
        public Task<int> CountRegistration(int id, int idLaunch);
        public Task<int> TotalProductOfLaunch (int idLaunch);
        public Task<Product> Detail(int id);
    }
}

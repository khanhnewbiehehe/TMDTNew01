using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Areas.Customer.Services
{
    public interface IFavouriteCustomerService
    {
        Task<IEnumerable<Product>> List(string id);
        Task<bool> Create(int id, string FavouriteId);
        Task<bool> Delete(int id, string FavouriteId);
        Task<Favourite> Find(string id);
        Task<bool> Init(string id);
    }
}

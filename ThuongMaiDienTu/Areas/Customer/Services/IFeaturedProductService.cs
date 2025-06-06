using ThuongMaiDienTu.ViewModels;

namespace ThuongMaiDienTu.Areas.Customer.Services
{
    public interface IFeaturedProductService
    {
        Task<ProductCardVM> GetFeaturedProduct();
    }
}

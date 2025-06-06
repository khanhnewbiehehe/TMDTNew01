using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Services
{
    public interface ICategoryService
    {
        public Task<IEnumerable<Category>> List();
        public Task<Category> Detail(int id);
    }
}

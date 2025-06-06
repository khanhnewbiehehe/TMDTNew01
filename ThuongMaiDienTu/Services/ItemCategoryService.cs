using Microsoft.EntityFrameworkCore;
using ThuongMaiDienTu.Data;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Services
{
    public class ItemCategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public ItemCategoryService(AppDbContext context)
        {
            _context = context;
        }

        public Task<Category> Detail(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> List()
        {
            var list = await _context.Categories.ToListAsync();
            return list;
        }
    }
}

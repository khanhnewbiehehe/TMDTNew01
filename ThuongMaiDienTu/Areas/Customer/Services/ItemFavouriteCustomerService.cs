using Microsoft.EntityFrameworkCore;
using ThuongMaiDienTu.Data;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Areas.Customer.Services
{
    public class ItemFavouriteCustomerService : IFavouriteCustomerService
    {
        private readonly AppDbContext _context;

        public ItemFavouriteCustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(int id, string FavouriteId)
        {
            try
            {
                var item = await _context.FavouriteProducts.FirstOrDefaultAsync(x => x.ProductId == id && x.FavouriteId == FavouriteId);
                if (item == null)
                {
                    var product = new FavouriteProduct();
                    product.ProductId = id;
                    product.FavouriteId = FavouriteId;
                    _context.FavouriteProducts.Add(product);
                    return await _context.SaveChangesAsync() > 0;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id, string FavouriteId)
        {
            try
            {
                var item = await _context.FavouriteProducts.FirstOrDefaultAsync(x => x.ProductId == id && x.FavouriteId == FavouriteId);
                if (item == null)
                {
                    return false;
                }
                _context.FavouriteProducts.Remove(item);
                return await _context.SaveChangesAsync() > 0;
            }
            catch(Exception ex)
            {
                return false;
            } 
        }

        public async Task<Favourite> Find(string id)
        {
            var item = await _context.Favorites.FindAsync(id);
            if (item == null)
            {
                return null;
            }
            else
            {
                return item;
            }
        }

        public async Task<bool> Init(string id)
        {
            try
            {
                var item = new Favourite();
                item.UserId = id;
                _context.Favorites.Add(item);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Product>> List(string id)
        {
            var list = await _context.FavouriteProducts.Include(x => x.Product)
                                                        .ThenInclude(x => x.Launches)
                                                       .Include(x => x.Product)
                                                        .ThenInclude(x => x.Images)
                                                       .Where(x => x.FavouriteId == id)
                                                       .Select(x => x.Product)
                                                       .ToListAsync();
            return list;
        }
    }
}

namespace ThuongMaiDienTu.Models
{
    public class FavouriteProduct
    {
        public string FavouriteId {  get; set; }
        public Favourite Favourite { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime CreateAt { get; set; }
    }
}

namespace ThuongMaiDienTu.ViewModels
{
    public class ProductCardVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public DateTime End { get; set; }
        public int Registration { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
    }
}

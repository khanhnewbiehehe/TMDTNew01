namespace ThuongMaiDienTu.Areas.Admin.ViewModels
{
    public class ProductLaunchViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }
    }
}

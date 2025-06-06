using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuongMaiDienTu.Areas.Admin.ViewModels
{
    // ProductLaunchCreateViewModel.cs
    public class ProductLaunchCreateViewModel
    {
        // Thông tin đợt mở bán
        public int ?Id { get; set; } // Thêm Id để hỗ trợ chỉnh sửa
        [Required]
        public string LaunchName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        [Required]
        public DateTime DateEnd { get; set; }
        
        public int ProductId { get; set; }
        // Danh sách sản phẩm tham gia đợt mở bán
        public List<ProductTypeViewModel> ProductTypes { get; set; } = new List<ProductTypeViewModel>();
    }

    public class ProductTypeViewModel
    {
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        public IFormFile ?ImageFile { get; set; }

        public string ?ImageUrl { get; set; }
        public long MaxPrice { get; set; }

        public List<PriceItemViewModel> PriceItems { get; set; } = new List<PriceItemViewModel>();
    }

    public class PriceItemViewModel
    {
        [Required]
        public int Quantity { get; set; }
        [Required]
        public long Price { get; set; }
    }


}

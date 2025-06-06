using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuongMaiDienTu.Models
{
    public class ProductType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string? ImageUrl{ get; set; }
        [ValidateNever]
        public ICollection<PriceItem> Prices { get; set; }
        [ValidateNever]
        public ICollection<InvoiceItem> InvoiceItems { get; set; }
        [ForeignKey(nameof(ProductLaunch))]
        public int ProductLaunchId { get; set; }
        [ValidateNever]
        public ProductLaunch ProductLaunch { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinPrice {  get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxPrice { get; set; }

    }
}

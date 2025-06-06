using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuongMaiDienTu.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Description { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; }
    }
}

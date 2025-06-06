using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuongMaiDienTu.Models
{
    public class ProductLaunch
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        [Required]
        public DateTime DateEnd { get; set; }
        [ValidateNever]
        public ICollection<ProductType> Types { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId {  get; set; }
        [ValidateNever]
        public Product Product { get; set; }

    }
}

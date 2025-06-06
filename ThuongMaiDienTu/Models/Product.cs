using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuongMaiDienTu.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public ICollection<ProductImage> Images { get; set; }
        [ValidateNever]
        public ICollection<ProductLaunch> Launches { get; set; }
        [ValidateNever]
        public ICollection<FavouriteProduct> FavouriteProducts { get; set; }
    }
}

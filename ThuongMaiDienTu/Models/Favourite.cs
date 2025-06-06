using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ThuongMaiDienTu.Models
{
    public class Favourite
    {
        [Key]
        public string UserId { get; set; }
        [ValidateNever]
        public AppUser User { get; set; }
        [ValidateNever]
        public ICollection<FavouriteProduct> FavouriteProducts { get; set; }
    }
}

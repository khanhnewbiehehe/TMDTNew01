using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ThuongMaiDienTu.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public string? AvatarUrl { get; set; }
        [ValidateNever]
        public Favourite Favourite { get; set; }
    }
}

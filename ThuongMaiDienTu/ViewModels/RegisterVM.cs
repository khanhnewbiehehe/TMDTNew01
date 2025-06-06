using System.ComponentModel.DataAnnotations;

namespace ThuongMaiDienTu.ViewModels;

public class RegisterVM
{
    [Required]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "FirstName is required")]
    public string FullName { get; set; }
    [Compare("Password", ErrorMessage = "Passwords don't match.")]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
    public string? AvatarUrl { get; set; }

}

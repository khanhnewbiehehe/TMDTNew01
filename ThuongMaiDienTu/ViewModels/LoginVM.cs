using System.ComponentModel.DataAnnotations;

namespace ThuongMaiDienTu.ViewModels;

public class LoginVM
{
    [Required(ErrorMessage = "Email là bắt buộc.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Ghi nhớ tài khoản")]
    public bool RememberMe { get; set; }
}

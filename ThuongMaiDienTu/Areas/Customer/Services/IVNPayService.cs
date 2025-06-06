using ThuongMaiDienTu.Models;
using ThuongMaiDienTu.ViewModels;

namespace ThuongMaiDienTu.Areas.Customer.Services
{
    public interface IVNPayService
    {
        // Tạo URL thanh toán VNPay
        string CreatePaymentUrl(HttpContext context, decimal price, string note);

        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}

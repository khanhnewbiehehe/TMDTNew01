using System.Text;
using System.Security.Cryptography;
using ThuongMaiDienTu.ViewModels;
using ThuongMaiDienTu.Libraries;
using System.Security.Policy;
using Microsoft.VisualBasic;

namespace ThuongMaiDienTu.Areas.Customer.Services
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;

        public VNPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreatePaymentUrl(HttpContext context, decimal price, string note)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", _configuration["VNPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _configuration["VNPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _configuration["VNPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", ((int)(price * 100)).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _configuration["VNPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _configuration["VNPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", note);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["VNPay:ReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef",tick);
            var paymentUrl = vnpay.CreateRequestUrl(_configuration["VNPay:Url"], _configuration["VNPay:HashSecret"]);
            return paymentUrl;
        }

        public VnPaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key,value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
               
                Console.WriteLine($"Key: {key}, Value: {value}");
                
            }
            var vnp_TxnRef = vnpay.GetResponseData("vnp_TxnRef");
            Console.WriteLine("vnp_TxnRef: " + vnp_TxnRef);
            var vnp_OrderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _configuration["VNPay:HashSecret"]);
            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Success = false
                };
            }
            return new VnPaymentResponseModel 
            { 
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_OrderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode
                
            };
        }
    }
}

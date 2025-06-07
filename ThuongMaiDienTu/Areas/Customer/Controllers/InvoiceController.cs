using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;
using ThuongMaiDienTu.Areas.Customer.Models;
using ThuongMaiDienTu.Areas.Customer.Services;
using ThuongMaiDienTu.Data;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceCustomerService _invoice;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductTypeCustomerService _productType;
        private readonly IVNPayService _vnpayService;
        private readonly AppDbContext _context;

        public InvoiceController(IInvoiceCustomerService invoice, UserManager<AppUser> userManager, IProductTypeCustomerService productType, IVNPayService vnpayService, AppDbContext context)
        {
            _invoice = invoice;
            _userManager = userManager;
            _productType = productType;
            _vnpayService = vnpayService;
            _context = context;
        }

        [Route("Customer/Invoice/PreOrder")]
        public async Task<IActionResult> PreOrder([FromBody] List<InvoiceItem> invoiceItems)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                // Create a new HoaDonSanPham
                var hoaDon = new Invoice
                {
                    UserId = user.Id,
                    Status = -100,
                    Address = "",
                    CreateAt = DateTime.Now,
                    InvoiceItems = new List<InvoiceItem>()
                };

                foreach (var item in invoiceItems)
                {
                    var chiTiet = new InvoiceItem();
                    chiTiet.ProductTypeId = item.ProductTypeId;
                    chiTiet.Quantity = item.Quantity;
                    chiTiet.Amount = item.Amount;
                    hoaDon.InvoiceItems.Add(chiTiet);
                }

                // Store HoaDonSanPham in session
                HttpContext.Session.SetString("PendingHoaDon", JsonSerializer.Serialize(hoaDon));

                return Json(new { success = true, message = "Hóa đơn tạm thời đã được tạo!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }        [Route("Customer/Invoice/Create")]
        public async Task<IActionResult> Create()
        {
            var hoaDonJson = HttpContext.Session.GetString("PendingHoaDon");
            if (string.IsNullOrEmpty(hoaDonJson))
            {
                return RedirectToAction("Index", "Home");
            }
            var hoaDon = JsonSerializer.Deserialize<Invoice>(hoaDonJson);
            
            // Load ProductType details for each invoice item
            if (hoaDon?.InvoiceItems != null)
            {
                foreach (var item in hoaDon.InvoiceItems)
                {
                    if (item != null)
                    {
                        item.ProductType = await _productType.Details(item.ProductTypeId);
                    }
                }
            }
            
            return View(hoaDon);
        }

        [Route("Customer/Invoice/Create")]
        [HttpPost]
        public async Task<IActionResult> Create(Invoice model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
                return View(model);
            }
            model.PaymentCode = model.UserId + DateTime.Now.ToString();
            var result = await _invoice.Order(model);
            if (!result)
            {
                TempData["ErrorMessage"] = "Đặt mua thất bại!";
                return RedirectToAction("Index", "SanPham");
            }
            HttpContext.Session.Remove("PendingHoaDon");
            return Redirect(_vnpayService.CreatePaymentUrl(HttpContext, model.Deposit, model.Id.ToString()));
        }

        [Route("Customer/Invoice/Result")]
        public async Task<IActionResult> Result()
        {
            if (Request.Query.Keys.Any(k => k.StartsWith("vnp_")))
            {
                var response = _vnpayService.PaymentExecute(Request.Query);
                if (response == null || response.VnPayResponseCode != "00")
                {
                    ViewBag.Result = 0;
                    return View();
                }

                var code = response.OrderDescription;
                var hoaDon = await _context.Invoices
                    .FirstOrDefaultAsync(x => x.Id.ToString() == code);

                if (hoaDon == null)
                {
                    ViewBag.Result = 0;
                    return View();
                }

                if (hoaDon.Status == 0)
                {
                    hoaDon.Status = 1;
                }                if (hoaDon.Status == -100)
                {
                    hoaDon.Status = 0;
                    
                    // Giảm số lượng sản phẩm khi thanh toán thành công
                    await ReduceProductQuantities(hoaDon.Id);
                }

                _context.Invoices.Update(hoaDon);
                await _context.SaveChangesAsync();

                ViewBag.Result = 1;
                return View();
            }

            return View();
        }

        [Route("Customer/Invoice")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var list = await _invoice.ListByCustomer(user.Id);
            return View(list);
        }
        [Route("Customer/Invoice/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _invoice.Details(id);
            ViewBag.CurrentPrice = await _productType.CurrentPrice(item.InvoiceItems.FirstOrDefault().ProductTypeId);
            return View(item);
        }
        [Route("Customer/Invoice/Payment")]
        [HttpPost]
        public async Task<IActionResult> Payment([FromBody] Invoice invoice)
        {
            var redirectUrl = _vnpayService.CreatePaymentUrl(HttpContext, invoice.ToTal - invoice.Deposit, invoice.Id.ToString());
            return Ok(new { url = redirectUrl }); // ✅ Trả về URL dưới dạng JSON
        }        [Route("Customer/Invoice/Cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            // Lấy thông tin hóa đơn trước khi hủy để kiểm tra trạng thái
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice != null && (invoice.Status == 0 || invoice.Status == 1))
            {
                // Nếu hóa đơn đã thanh toán cọc hoặc đã thanh toán đủ, cần hoàn lại số lượng
                await RestoreProductQuantities(id);
            }

            var result = await _invoice.Cancel(id);
            return Json(result);
        }

        // Phương thức private để giảm số lượng sản phẩm
        private async Task ReduceProductQuantities(int invoiceId)
        {
            try
            {
                // Lấy thông tin hóa đơn với các item
                var invoice = await _context.Invoices
                    .Include(i => i.InvoiceItems)
                    .FirstOrDefaultAsync(i => i.Id == invoiceId);

                if (invoice?.InvoiceItems != null)
                {
                    foreach (var item in invoice.InvoiceItems)
                    {
                        // Lấy thông tin ProductType
                        var productType = await _context.ProductTypes
                            .FirstOrDefaultAsync(pt => pt.Id == item.ProductTypeId);

                        if (productType != null)
                        {
                            // Kiểm tra số lượng còn lại có đủ không
                            if (productType.Quantity >= item.Quantity)
                            {
                                productType.Quantity -= item.Quantity;
                                _context.ProductTypes.Update(productType);
                            }
                            else
                            {
                                // Log warning hoặc xử lý trường hợp không đủ hàng
                                System.Diagnostics.Debug.WriteLine($"Không đủ số lượng cho ProductType ID: {item.ProductTypeId}. Còn lại: {productType.Quantity}, Yêu cầu: {item.Quantity}");
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log lỗi                System.Diagnostics.Debug.WriteLine($"Lỗi khi giảm số lượng sản phẩm: {ex.Message}");
                throw;
            }
        }

        // Phương thức private để hoàn lại số lượng sản phẩm khi hủy đơn
        private async Task RestoreProductQuantities(int invoiceId)
        {
            try
            {
                // Lấy thông tin hóa đơn với các item
                var invoice = await _context.Invoices
                    .Include(i => i.InvoiceItems)
                    .FirstOrDefaultAsync(i => i.Id == invoiceId);

                if (invoice?.InvoiceItems != null)
                {
                    foreach (var item in invoice.InvoiceItems)
                    {
                        // Lấy thông tin ProductType
                        var productType = await _context.ProductTypes
                            .FirstOrDefaultAsync(pt => pt.Id == item.ProductTypeId);

                        if (productType != null)
                        {
                            // Hoàn lại số lượng
                            productType.Quantity += item.Quantity;
                            _context.ProductTypes.Update(productType);
                            System.Diagnostics.Debug.WriteLine($"Hoàn lại số lượng cho ProductType ID: {item.ProductTypeId}. Số lượng hoàn: {item.Quantity}, Tổng hiện tại: {productType.Quantity}");
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log lỗi
                System.Diagnostics.Debug.WriteLine($"Lỗi khi hoàn lại số lượng sản phẩm: {ex.Message}");
                throw;
            }
        }
    }
}


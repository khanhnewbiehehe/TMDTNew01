using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuongMaiDienTu.Models
{
    public class InvoiceItem
    {
        public int InvoiceId { get; set; }
        [ValidateNever]
        public Invoice Invoice { get; set; }
        public int ProductTypeId { get; set; }
        [ValidateNever]
        public ProductType ProductType { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}

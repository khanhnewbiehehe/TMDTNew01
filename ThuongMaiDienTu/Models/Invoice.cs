using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuongMaiDienTu.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [Required]
        public string Address { get; set; }
        public int Status {  get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Deposit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ToTal { get; set; } 
        public string PaymentCode { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }
        [ValidateNever]
        public AppUser User { get; set; }
        [ValidateNever]
        public ICollection<InvoiceItem> InvoiceItems { get; set; }  
    }
}

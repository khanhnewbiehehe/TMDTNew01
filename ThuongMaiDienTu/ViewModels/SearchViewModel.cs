using ThuongMaiDienTu.Models;
using System.Collections.Generic;

namespace ThuongMaiDienTu.ViewModels
{
    public class SearchViewModel
    {
        public string SearchTerm { get; set; } = "";
        public int CategoryId { get; set; } = 0;
        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set; } = decimal.MaxValue;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public bool LaunchGoing { get; set; } = true;
        public bool LaunchFull { get; set; } = true;
        public bool LaunchEnded { get; set; } = true;
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<ProductCardVM> Products { get; set; } = new List<ProductCardVM>();
        public int TotalPages => (int)Math.Ceiling((double)Products.Count() / PageSize);
    }
}
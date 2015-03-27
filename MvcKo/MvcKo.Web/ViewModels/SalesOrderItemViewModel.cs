
using System.ComponentModel.DataAnnotations;
namespace MvcKo.Web.ViewModels
{
    public class SalesOrderItemViewModel: ViewModel
    {
        public int SalesOrderItemId { get; set; }
        [Required]
        [StringLength(30)]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "The quantity is required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "The unit price is required")]
        public decimal UnitPrice { get; set; }
        public int SalesOrderId { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcKo.Web.ViewModels
{
    public class SalesOrderViewModel: ViewModel
    {
        public SalesOrderViewModel()
        {
            SalesOrderItems = new List<SalesOrderItemViewModel>();
            SalesOrderItemsToDelete = new List<int>();
        }

        public int SalesOrderId { get; set; }
        [Required(ErrorMessage = "The order must have a customer name.")]
        [StringLength(30, ErrorMessage = "The customer name must have between 1-30 characters.")]
        public string CustomerName { get; set; }
        [StringLength(10)]
        public string PoNumber { get; set; }
        public string MessageToClient { get; set; }
        public List<SalesOrderItemViewModel> SalesOrderItems { get; set; }
        public List<int> SalesOrderItemsToDelete { get; set; }
    }
}
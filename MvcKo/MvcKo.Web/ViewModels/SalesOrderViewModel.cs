using System.Collections.Generic;

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
        public string CustomerName { get; set; }
        public string PoNumber { get; set; }
        public string MessageToClient { get; set; }
        public List<SalesOrderItemViewModel> SalesOrderItems { get; set; }
        public List<int> SalesOrderItemsToDelete { get; set; }
    }
}
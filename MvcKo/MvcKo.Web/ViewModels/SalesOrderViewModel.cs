using System.Collections.Generic;

namespace MvcKo.Web.ViewModels
{
    public class SalesOrderViewModel: ViewModel
    {
        public SalesOrderViewModel()
        {
            SalesOrderItems = new List<SalesOrderItemViewModel>();
        }

        public int SalesOrderId { get; set; }
        public string CustomerName { get; set; }
        public string PoNumber { get; set; }
        public string MessageToClient { get; set; }
        public List<SalesOrderItemViewModel> SalesOrderItems { get; set; }
    }
}
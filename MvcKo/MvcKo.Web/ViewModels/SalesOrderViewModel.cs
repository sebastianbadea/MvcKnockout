
namespace MvcKo.Web.ViewModels
{
    public class SalesOrderViewModel: ViewModel
    {
        public int SalesOrderId { get; set; }
        public string CustomerName { get; set; }
        public string PoNumber { get; set; }
        public string MessageToClient { get; set; }
    }
}
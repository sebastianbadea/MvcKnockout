
namespace MvcKo.Web.ViewModels
{
    public class SalesOrderItemViewModel: ViewModel
    {
        public int SalesOrderItemId { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int SalesOrderId { get; set; }
    }
}
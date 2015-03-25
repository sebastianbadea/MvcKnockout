
namespace MvcKo.Model
{
    public class SalesOrder: Entity
    {
        public int SalesOrderId { get; set; }
        public string CustomerName { get; set; }
        public string PoNumber { get; set; }
    }
}

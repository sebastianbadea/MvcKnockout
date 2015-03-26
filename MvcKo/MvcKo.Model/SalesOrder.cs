using System.Collections.Generic;

namespace MvcKo.Model
{
    public class SalesOrder: Entity
    {
        public SalesOrder()
        {
            SalesOrderItems = new List<SalesOrderItem>();
        }

        public int SalesOrderId { get; set; }
        public string CustomerName { get; set; }
        public string PoNumber { get; set; }
        public virtual List<SalesOrderItem> SalesOrderItems { get; set; }
    }
}

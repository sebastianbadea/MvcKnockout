using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcKo.Web.ViewModels
{
    public class SalesOrderViewModel
    {
        public int SalesOrderId { get; set; }
        public string CustomerName { get; set; }
        public string PoNumber { get; set; }
        public string MessageToClient { get; set; }
    }
}
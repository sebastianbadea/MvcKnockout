using MvcKo.Model;
using System.Data.Entity;

namespace MvcKo.DataLayer
{
    public class SalesContext: DbContext
    {
        public SalesContext(): base("DefaultConnection") { }

        public IDbSet<SalesOrder> SalesOrders { get; set; }
        public IDbSet<SalesOrderItem> SalesOrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SalesOrderConfiguration());
            modelBuilder.Configurations.Add(new SalesOrderItemConfiguration());
        }
    }
}

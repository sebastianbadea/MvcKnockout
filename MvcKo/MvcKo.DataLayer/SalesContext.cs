using MvcKo.Model;
using System.Data.Entity;

namespace MvcKo.DataLayer
{
    public class SalesContext: DbContext
    {
        public SalesContext(): base("DefaultConnection") { }

        public DbSet<SalesOrder> SalesOrders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SalesOrderConfiguration());
        }
    }
}

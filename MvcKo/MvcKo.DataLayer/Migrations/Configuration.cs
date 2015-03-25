using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using MvcKo.DataLayer;
using MvcKo.Model;

namespace MvcKo.DataLayer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SalesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SalesContext context)
        {

            context.SalesOrders.AddOrUpdate
            (
                so => so.CustomerName,
                new SalesOrder { CustomerName = "Mike", PoNumber = "1" },
                new SalesOrder { CustomerName = "Adam" },
                new SalesOrder { CustomerName = "John", PoNumber = "2" }
            );
        }

        
    }
}

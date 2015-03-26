using MvcKo.Model;
using System.Data.Entity.ModelConfiguration;

namespace MvcKo.DataLayer
{
    class SalesOrderItemConfiguration : EntityTypeConfiguration<SalesOrderItem>
    {
        public SalesOrderItemConfiguration()
        {
            Property(soi => soi.ProductCode).HasMaxLength(30).IsRequired();
            Property(soi => soi.Quantity).IsRequired();
            Property(soi => soi.UnitPrice).IsRequired();
            Ignore(soi => soi.State);
        }
    }
}

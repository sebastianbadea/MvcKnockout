using MvcKo.Model;
using System.Data.Entity.ModelConfiguration;

namespace MvcKo.DataLayer
{
    public class SalesOrderConfiguration: EntityTypeConfiguration<SalesOrder>
    {
        public SalesOrderConfiguration()
        {
            Property(so => so.CustomerName).HasMaxLength(30).IsRequired();
            Property(so => so.PoNumber).HasMaxLength(10).IsOptional();
        }
    }
}

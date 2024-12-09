using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Core.DataAccess.Configuration;
using SM.WarehouseMgmt.Domain.Concrete;

namespace SM.WarehouseMgmt.DataAccess.Configurations
{
    public class WarehouseEntityConfiguration : EntityDefaultConfigurationBase<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.Address).HasMaxLength(500);
            builder.Property(x => x.ResponsiblePerson);
        }
    }
}

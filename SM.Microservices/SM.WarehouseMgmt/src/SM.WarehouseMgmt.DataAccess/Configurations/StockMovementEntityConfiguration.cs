using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Core.DataAccess.Configuration;
using SM.WarehouseMgmt.Domain.Concrete;

namespace SM.WarehouseMgmt.DataAccess.Configurations
{
    public class StockMovementEntityConfiguration : EntityDefaultConfigurationBase<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.MovementType);
            builder.Property(x => x.Quantity);
            builder.Property(x => x.Date);
        }
    }
}

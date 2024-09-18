using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Core.DataAccess.Configuration;
using SM.ProductMgmt.Domain.Concrete;

namespace SM.ProductMgmt.DataAccess.Configurations
{
    public class ProductEntityConfiguration : EntityDefaultConfigurationBase<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Price);
            builder.Property(x => x.StockQuantity);
            builder.Property(x => x.Unit);
        }
    }
}

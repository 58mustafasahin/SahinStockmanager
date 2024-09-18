using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Core.DataAccess.Configuration;
using SM.ProductMgmt.Domain.Concrete;

namespace SM.ProductMgmt.DataAccess.Configurations
{
    public class CategoryEntityConfiguration : EntityDefaultConfigurationBase<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(500);
        }
    }
}

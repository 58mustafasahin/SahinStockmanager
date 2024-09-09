using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.AuthorizationMgmt.Domain.Concrete;
using SM.Core.DataAccess.Configuration;

namespace SM.AuthorizationMgmt.DataAccess.Configurations
{
    public class RoleEntityConfiguration : EntityDefaultConfigurationBase<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(500);
        }
    }
}

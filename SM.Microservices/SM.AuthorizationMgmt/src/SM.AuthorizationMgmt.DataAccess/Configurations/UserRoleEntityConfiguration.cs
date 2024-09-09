using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.AuthorizationMgmt.Domain.Concrete;
using SM.Core.DataAccess.Configuration;

namespace SM.AuthorizationMgmt.DataAccess.Configurations
{
    public class UserRoleEntityConfiguration : EntityDefaultConfigurationBase<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.UserId);
            builder.Property(x => x.RoleId);
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.AuthorizationMgmt.Domain.Concrete;
using SM.Core.DataAccess.Configuration;

namespace SM.AuthorizationMgmt.DataAccess.Configurations
{
    public class UserEntityConfiguration : EntityDefaultConfigurationBase<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.Surname).HasMaxLength(100);
            builder.Property(x => x.Email).HasMaxLength(100).IsRequired(false);
            builder.Property(x => x.Username).HasMaxLength(100);
            builder.Property(x => x.BirthDate);
            builder.Property(x => x.MobilePhone).HasMaxLength(30);
            builder.Property(x => x.GenderId);

            builder.HasIndex(x => x.CitizenId);
            builder.HasIndex(x => x.MobilePhone);

            builder.HasIndex(x => x.CitizenId).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
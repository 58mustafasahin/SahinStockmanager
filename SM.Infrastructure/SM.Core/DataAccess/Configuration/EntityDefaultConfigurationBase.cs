using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Core.Domain.Entities;
using System.Globalization;

namespace SM.Core.DataAccess.Configuration
{
    public abstract class EntityDefaultConfigurationBase<TEntity> : BaseConfigurationBase<TEntity>
        where TEntity : EntityDefault, new()
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .UseIdentityColumn();

            builder.Property(e => e.InsertTime)
                .HasDefaultValueSql("NOW()");
        }
    }

    public abstract class BaseConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            var tableName = builder.GetType().GetGenericArguments()[0].Name;
            builder.ToTable(tableName.ToLower(new CultureInfo("en-US", false)));
        }
    }
}

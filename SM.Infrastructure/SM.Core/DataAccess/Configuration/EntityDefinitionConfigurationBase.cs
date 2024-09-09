using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SM.Core.Domain.Entities;

namespace SM.Core.DataAccess.Configuration
{
    public abstract class EntityDefinitionConfigurationBase<TEntity> : EntityDefaultConfigurationBase<TEntity>
       where TEntity : EntityDefinition, new()
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Code).HasMaxLength(50);
        }
    }
}

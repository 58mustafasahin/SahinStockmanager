using SM.Core.Domain.Enums;

namespace SM.Core.Domain.Entities
{
    public abstract class EntityDefinition : EntityDefault
    {
        public RecordStatus RecordStatus { get; set; } = RecordStatus.Active;
        public string Name { get; set; }
        public string Code { get; set; }
    }
}

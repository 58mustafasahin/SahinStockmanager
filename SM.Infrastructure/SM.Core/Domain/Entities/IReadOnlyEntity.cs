namespace SM.Core.Domain.Entities
{
    public interface IReadOnlyEntity : IEntity
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}

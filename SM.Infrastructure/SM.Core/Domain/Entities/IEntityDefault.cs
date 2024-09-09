namespace SM.Core.Domain.Entities
{
    public interface IEntityDefault : IEntity
    {
        public long Id { get; set; }
        public DateTime InsertTime { get; set; }
        public long? InsertUserId { get; set; }
        public DateTime UpdateTime { get; set; }
        public long? UpdateUserId { get; set; }
        public bool IsDeleted { get; set; }
    }
}

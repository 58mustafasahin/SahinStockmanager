using System.Text.Json.Serialization;

namespace SM.Core.Domain.Entities
{
    public class EntityDefault : IEntityDefault
    {
        public long Id { get; set; }
        [JsonIgnore]
        public DateTime InsertTime { get; set; }
        [JsonIgnore]
        public long? InsertUserId { get; set; }
        [JsonIgnore]
        public DateTime UpdateTime { get; set; }
        [JsonIgnore]
        public long? UpdateUserId { get; set; }
        [JsonIgnore]
        public bool IsDeleted { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;

namespace SM.Core.Domain.Dtos
{
    public class ChangeTrackState
    {
        public int Index { get; set; }
        public EntityState State { get; set; }
    }
}

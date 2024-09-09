using Microsoft.EntityFrameworkCore.ChangeTracking;
using SM.Core.Domain.Dtos;

namespace SM.Core.Services.EntityChangeServices.Model
{
    public class ChangeEntityModel
    {
        public List<ChangeTrackState> ChangeTrackStates { get; set; }
        public EntityEntry[] ChangeTrack { get; set; }
    }
}

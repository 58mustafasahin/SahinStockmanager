using SM.Core.Services.EntityChangeServices.Model;

namespace SM.Core.Services.EntityChangeServices
{
    public class EntityChangeServices : IEntityChangeServices
    {
        public EntityChangeServices()
        {
        }

        public List<ChangeEntityModel> ChangeEntriesList { get; set; } = new();
    }
}

using SM.Core.Services.EntityChangeServices.Model;

namespace SM.Core.Services.EntityChangeServices
{
    public interface IEntityChangeServices
    {
        public List<ChangeEntityModel> ChangeEntriesList { get; set; }
    }
}

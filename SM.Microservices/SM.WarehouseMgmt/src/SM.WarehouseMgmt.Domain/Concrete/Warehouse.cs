using SM.Core.Domain.Entities;

namespace SM.WarehouseMgmt.Domain.Concrete
{
    public class Warehouse : EntityDefinition
    {
        public string Address { get; set; }
        public string ResponsiblePerson { get; set; }
    }
}

using SM.Core.Domain.Entities;

namespace SM.ProductMgmt.Domain.Concrete
{
    public class Category : EntityDefinition
    {
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}

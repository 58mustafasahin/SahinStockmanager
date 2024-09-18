using SM.Core.Domain.Entities;

namespace SM.ProductMgmt.Domain.Concrete
{
    public class Product : EntityDefinition
    {
        public string Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public int Unit { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

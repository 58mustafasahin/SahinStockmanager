using SM.Core.Domain.Entities;
using SM.WarehouseMgmt.Domain.Enums;

namespace SM.WarehouseMgmt.Domain.Concrete
{
    public class StockMovement : EntityDefault
    {
        public MovementType MovementType { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public long ProductId { get; set; }
        public long WarehouseId { get; set; }
    }
}

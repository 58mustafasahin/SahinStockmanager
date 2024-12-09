using SM.Core.DataAccess;
using SM.WarehouseMgmt.Domain.Concrete;

namespace SM.WarehouseMgmt.DataAccess.Abstract
{
    public interface IStockMovementRepository : IEntityDefaultRepository<StockMovement>
    {
    }
}

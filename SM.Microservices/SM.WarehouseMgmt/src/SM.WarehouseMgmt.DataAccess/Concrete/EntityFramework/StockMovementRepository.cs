using SM.Core.DataAccess.EntityFramework;
using SM.WarehouseMgmt.DataAccess.Abstract;
using SM.WarehouseMgmt.DataAccess.Contexts;
using SM.WarehouseMgmt.Domain.Concrete;

namespace SM.WarehouseMgmt.DataAccess.Concrete.EntityFramework
{
    public class StockMovementRepository : EfEntityRepositoryBase<StockMovement, WarehouseDbContext>, IStockMovementRepository
    {
        public StockMovementRepository(WarehouseDbContext context) : base(context)
        {
        }
    }
}

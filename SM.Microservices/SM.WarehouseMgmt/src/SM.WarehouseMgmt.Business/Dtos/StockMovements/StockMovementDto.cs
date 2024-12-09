using SM.WarehouseMgmt.Domain.Enums;

namespace SM.WarehouseMgmt.Business.Dtos.StockMovements
{
    public record GetStockMovementDto(long Id, MovementType MovementType, int Quantity, DateTime Date, long ProductId, long WarehouseId);
}

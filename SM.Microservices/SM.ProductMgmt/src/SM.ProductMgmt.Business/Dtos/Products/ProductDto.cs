namespace SM.ProductMgmt.Business.Dtos.Products
{
    public record GetProductDto(long Id, string Name, string Description, double Price, int StockQuantity, int Unit);
}

using Microsoft.AspNetCore.Mvc;
using SM.ProductMgmt.Business.Features.Products.Queries;

namespace SM.ProductMgmt.Api.Controllers.Products
{
    public partial class ProductController
    {
        [HttpPost("api/product/getbyfilterpaged")]
        public async Task<ActionResult> GetProductByFilterPaged([FromBody] GetByFilterPagedProductsQuery command)
        {
            var result = await _mediator.Send(command);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

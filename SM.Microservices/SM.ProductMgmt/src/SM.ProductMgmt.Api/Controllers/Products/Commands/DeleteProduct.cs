using Microsoft.AspNetCore.Mvc;
using SM.ProductMgmt.Business.Features.Products.Commands;

namespace SM.ProductMgmt.Api.Controllers.Products
{
    public partial class ProductController
    {
        [HttpDelete("api/product/delete")]
        public async Task<ActionResult> DeleteProduct([FromQuery] DeleteProductCommand command)
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

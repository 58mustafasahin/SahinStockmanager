using Microsoft.AspNetCore.Mvc;
using SM.ProductMgmt.Business.Features.Products.Commands;

namespace SM.ProductMgmt.Api.Controllers.Products
{
    public partial class ProductController
    {
        [HttpPut("api/product/update")]
        public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
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

using Microsoft.AspNetCore.Mvc;
using SM.ProductMgmt.Business.Features.Products.Commands;

namespace SM.ProductMgmt.Api.Controllers.Products
{
    public partial class ProductController
    {
        [HttpPost("api/product/create")]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductCommand command)
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

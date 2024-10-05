using Microsoft.AspNetCore.Mvc;
using SM.ProductMgmt.Business.Features.Products.Queries;

namespace SM.ProductMgmt.Api.Controllers.Products
{
    public partial class ProductController
    {
        [HttpGet("api/product/getbyid")]
        public async Task<ActionResult> GetProductById([FromQuery] GetProductByIdQuery command)
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

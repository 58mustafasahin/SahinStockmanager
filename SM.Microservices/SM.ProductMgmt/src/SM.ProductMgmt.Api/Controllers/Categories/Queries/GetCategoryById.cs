using Microsoft.AspNetCore.Mvc;
using SM.ProductMgmt.Business.Features.Categories.Queries;

namespace SM.ProductMgmt.Api.Controllers.Categories
{
    public partial class CategoryController
    {
        [HttpGet("api/category/getbyid")]
        public async Task<ActionResult> GetCategoryById([FromQuery] GetCategoryByIdQuery command)
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

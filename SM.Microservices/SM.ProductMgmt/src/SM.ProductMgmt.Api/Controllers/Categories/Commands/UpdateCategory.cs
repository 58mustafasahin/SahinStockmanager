using Microsoft.AspNetCore.Mvc;
using SM.ProductMgmt.Business.Features.Categories.Commands;

namespace SM.ProductMgmt.Api.Controllers.Categories
{
    public partial class CategoryController
    {
        [HttpPut("api/category/update")]
        public async Task<ActionResult> UpdateCategory([FromBody] UpdateCategoryCommand command)
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

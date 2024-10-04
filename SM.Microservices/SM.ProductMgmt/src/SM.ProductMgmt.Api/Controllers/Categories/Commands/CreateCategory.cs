using Microsoft.AspNetCore.Mvc;
using SM.ProductMgmt.Business.Features.Categories.Commands;

namespace SM.ProductMgmt.Api.Controllers.Categories
{
    public partial class CategoryController
    {
        [HttpPost("api/category/create")]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
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

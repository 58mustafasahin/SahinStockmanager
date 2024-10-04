using Microsoft.AspNetCore.Mvc;
using SM.ProductMgmt.Business.Features.Categories.Commands;

namespace SM.ProductMgmt.Api.Controllers.Categories
{
    public partial class CategoryController
    {
        [HttpDelete("api/category/delete")]
        public async Task<ActionResult> DeleteCategory([FromQuery] DeleteCategoryCommand command)
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

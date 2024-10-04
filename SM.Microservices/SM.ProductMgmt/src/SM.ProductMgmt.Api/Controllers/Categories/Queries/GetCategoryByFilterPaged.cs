using Microsoft.AspNetCore.Mvc;
using SM.ProductMgmt.Business.Features.Categories.Queries;

namespace SM.ProductMgmt.Api.Controllers.Categories
{
    public partial class CategoryController
    {
        [HttpPost("api/category/getbyfilterpaged")]
        public async Task<ActionResult> GetCategoryByFilterPaged([FromBody] GetByFilterPagedCategoriesQuery command)
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

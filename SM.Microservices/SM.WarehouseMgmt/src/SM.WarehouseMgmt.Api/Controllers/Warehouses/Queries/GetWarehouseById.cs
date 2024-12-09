using Microsoft.AspNetCore.Mvc;
using SM.WarehouseMgmt.Business.Features.Warehouses.Queries;

namespace SM.WarehouseMgmt.Api.Controllers.Warehouses
{
    public partial class WarehouseController
    {
        [HttpGet("api/warehouse/getbyid")]
        public async Task<ActionResult> GetWarehouseById([FromQuery] GetWarehouseByIdQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SM.WarehouseMgmt.Business.Features.Warehouses.Queries;

namespace SM.WarehouseMgmt.Api.Controllers.Warehouses
{
    public partial class WarehouseController
    {
        [HttpPost("api/warehouse/getbyfilterpaged")]
        public async Task<ActionResult> GetWarehouseByFilterPaged([FromBody] GetByFilterPagedWarehousesQuery query)
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

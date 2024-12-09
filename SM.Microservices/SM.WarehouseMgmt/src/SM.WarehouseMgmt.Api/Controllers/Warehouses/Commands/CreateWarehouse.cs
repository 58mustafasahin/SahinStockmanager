using Microsoft.AspNetCore.Mvc;
using SM.WarehouseMgmt.Business.Features.Warehouses.Commands;

namespace SM.WarehouseMgmt.Api.Controllers.Warehouses
{
    public partial class WarehouseController
    {
        [HttpPost("api/warehouse/create")]
        public async Task<ActionResult> CreateWarehouse([FromBody] CreateWarehouseCommand command)
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


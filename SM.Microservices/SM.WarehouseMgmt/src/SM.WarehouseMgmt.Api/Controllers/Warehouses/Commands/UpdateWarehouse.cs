using Microsoft.AspNetCore.Mvc;
using SM.WarehouseMgmt.Business.Features.Warehouses.Commands;

namespace SM.WarehouseMgmt.Api.Controllers.Warehouses
{
    public partial class WarehouseController
    {
        [HttpPut("api/warehouse/update")]
        public async Task<ActionResult> UpdateWarehouse([FromBody] UpdateWarehouseCommand command)
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

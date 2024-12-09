using Microsoft.AspNetCore.Mvc;
using SM.WarehouseMgmt.Business.Features.Warehouses.Commands;

namespace SM.WarehouseMgmt.Api.Controllers.Warehouses
{
    public partial class WarehouseController
    {
        [HttpDelete("api/warehouse/delete")]
        public async Task<ActionResult> DeleteWarehouse([FromQuery] DeleteWarehouseCommand command)
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

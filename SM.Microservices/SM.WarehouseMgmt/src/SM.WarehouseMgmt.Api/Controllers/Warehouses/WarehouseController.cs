using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SM.WarehouseMgmt.Api.Controllers.Warehouses
{
    [ApiController]
    public partial class WarehouseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WarehouseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}

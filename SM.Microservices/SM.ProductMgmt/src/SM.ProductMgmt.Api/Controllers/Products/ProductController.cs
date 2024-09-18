using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SM.ProductMgmt.Api.Controllers.Products
{
    [ApiController]
    public partial class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}

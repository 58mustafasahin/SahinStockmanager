using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SM.ProductMgmt.Api.Controllers.Categories
{
    [ApiController]
    public partial class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SM.AuthorizationMgmt.Api.Controllers.Authorizations
{
    [ApiController]
    public partial class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SM.AuthorizationMgmt.Business.Features.Authorizations.Commands;

namespace SM.AuthorizationMgmt.Api.Controllers.Authorizations
{
    public partial class AuthController
    {
        [HttpPost("api/auth/register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserCommand command)
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
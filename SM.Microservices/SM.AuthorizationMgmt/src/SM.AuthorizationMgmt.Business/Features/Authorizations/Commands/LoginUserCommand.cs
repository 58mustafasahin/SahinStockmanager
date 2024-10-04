using MediatR;
using SM.AuthorizationMgmt.Business.Dtos.Authorizations;
using SM.AuthorizationMgmt.Business.Services.Authorizations;
using SM.AuthorizationMgmt.DataAccess.Abstract;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.Core.Utilities.Security.Hashing;

namespace SM.AuthorizationMgmt.Business.Features.Authorizations.Commands
{
    public class LoginUserCommand : IRequest<IResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, IResult>
        {
            private readonly IUserRepository _userRepository;
            private readonly IAuthService _authService;
            public LoginUserCommandHandler(IUserRepository userRepository, IAuthService authService)
            {
                _userRepository = userRepository;
                _authService = authService;
            }

            public async Task<IResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetAsync(x => x.Username == request.Username);
                if (user is null || !HashingHelper.VerifyPasswordHash(request.Password, user.PasswordSalt, user.PasswordHash))
                    return new ErrorDataResult<AuthResultDto>(Messages.UsernameOrPasswordIsWrong);

                var token = _authService.GenerateToken(user);

                return new SuccessDataResult<AuthResultDto>(token, Messages.SuccessfulOperation);
            }
        }
    }
}

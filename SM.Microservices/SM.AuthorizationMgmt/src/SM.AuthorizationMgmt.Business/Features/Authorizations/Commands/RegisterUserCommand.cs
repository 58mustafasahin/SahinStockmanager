using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SM.AuthorizationMgmt.Business.Dtos.Authorizations;
using SM.AuthorizationMgmt.Business.Services.Authorizations;
using SM.AuthorizationMgmt.DataAccess.Abstract;
using SM.AuthorizationMgmt.Domain.Concrete;
using SM.Core.Common.Constants;
using SM.Core.Utilities.Results;
using SM.Core.Utilities.Security.Hashing;

namespace SM.AuthorizationMgmt.Business.Features.Authorizations.Commands
{
    public class RegisterUserCommand : IRequest<DataResult<AuthResultDto>>
    {
        public string Username { get; set; }
        public long CitizenId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, DataResult<AuthResultDto>>
        {
            private readonly IUserRepository _userRepository;
            private readonly IAuthService _authService;
            public RegisterUserCommandHandler(IUserRepository userRepository, IAuthService authService)
            {
                _userRepository = userRepository;
                _authService = authService;
            }

            public async Task<DataResult<AuthResultDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                if (request.Password.Equals(request.ConfirmPassword, StringComparison.OrdinalIgnoreCase))
                    return new ErrorDataResult<AuthResultDto>(Messages.PasswordsDoNotMatch);

                var citizenIdCheck = await _userRepository.GetAsync(w => w.Status && w.CitizenId == request.CitizenId && w.CitizenId != 0);
                if (citizenIdCheck is not null)
                    return new ErrorDataResult<AuthResultDto>(Messages.CitizenIdAlreadyExist);

                var existEmail = await _userRepository.Query().AnyAsync(x => x.Email.Equals(request.Email, StringComparison.CurrentCultureIgnoreCase));
                if (existEmail)
                    return new ErrorDataResult<AuthResultDto>(Messages.EmailAlreadyExist);

                var existPhone = await _userRepository.Query().AnyAsync(x => x.MobilePhone == request.MobilePhone);
                if (existPhone)
                    return new ErrorDataResult<AuthResultDto>(Messages.MobilePhoneAlreadyExist);

                HashingHelper.CreatePasswordHash(request.Password, out var passwordSalt, out var passwordHash);

                var user = request.Adapt<User>();
                user.PasswordSalt = passwordSalt;
                user.PasswordHash = passwordHash;

                _userRepository.Add(user);
                await _userRepository.SaveChangesAsync();

                var token = _authService.GenerateToken(user);

                return new SuccessDataResult<AuthResultDto>(token, Messages.SuccessfulOperation);
            }
        }
    }
}

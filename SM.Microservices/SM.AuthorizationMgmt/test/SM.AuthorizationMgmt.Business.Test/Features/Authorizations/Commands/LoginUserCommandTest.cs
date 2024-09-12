using FluentAssertions;
using MockQueryable;
using Moq;
using NUnit.Framework;
using SM.AuthorizationMgmt.Business.Dtos.Authorizations;
using SM.AuthorizationMgmt.Business.Features.Authorizations.Commands;
using SM.AuthorizationMgmt.Business.Services.Authorizations;
using SM.AuthorizationMgmt.DataAccess.Abstract;
using SM.AuthorizationMgmt.Domain.Concrete;
using SM.Core.Utilities.Security.Hashing;
using System.Linq.Expressions;

namespace SM.AuthorizationMgmt.Business.Test.Features.Authorizations.Commands
{
    [TestFixture]
    public class LoginUserCommandTest
    {
        private LoginUserCommand _loginUserCommand;
        private LoginUserCommand.LoginUserCommandHandler _loginUserCommandHandler;

        private Mock<IUserRepository> _userRepository;
        private Mock<IAuthService> _authService;

        List<User> _fakeUsers;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _authService = new Mock<IAuthService>();

            _loginUserCommand = new LoginUserCommand();
            _loginUserCommandHandler = new LoginUserCommand.LoginUserCommandHandler(_userRepository.Object, _authService.Object);

            HashingHelper.CreatePasswordHash("123", out var passwordSalt, out var passwordHash);
            _fakeUsers = new()
            {
                new User
                {
                    Username = "Test",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                },
                new User
                {
                    Username = "John",

                },
            };
        }

        [Test]
        public async Task LoginUser_Success()
        {
            _loginUserCommand = new()
            {
                Username = "Test",
                Password = "123"
            };
            var user = _fakeUsers.FirstOrDefault(x => x.Username == _loginUserCommand.Username);
            _userRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user);

            _authService.Setup(x => x.GenerateToken(user)).Returns(new AuthResultDto("", DateTime.UtcNow));

            var result = await _loginUserCommandHandler.Handle(_loginUserCommand, CancellationToken.None);
            result.Success.Should().BeTrue();
        }

        [Test]
        [TestCase("AAA", "123")]
        [TestCase("Test", "321")]
        public async Task LoginUser_Fail_UsernameOrPasswordIsWrong(string userName, string password)
        {
            _loginUserCommand = new()
            {
                Username = userName,
                Password = password
            };

            _userRepository.Setup(x => x.Query()).Returns(_fakeUsers.AsQueryable().BuildMock());

            _authService.Setup(x => x.GenerateToken(new User())).Returns(new AuthResultDto("", DateTime.UtcNow));

            var result = await _loginUserCommandHandler.Handle(_loginUserCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
        }
    }
}

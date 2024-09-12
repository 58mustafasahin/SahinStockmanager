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

namespace SM.AuthorizationMgmt.Business.Test.Features.Authorizations.Commands
{
    [TestFixture]
    public class RegisterUserCommandTest
    {
        private RegisterUserCommand _registerUserCommand;
        private RegisterUserCommand.RegisterUserCommandHandler _registerUserCommandHandler;

        private Mock<IUserRepository> _userRepository;
        private Mock<IAuthService> _authService;

        List<User> _fakeUsers;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _authService = new Mock<IAuthService>();

            _registerUserCommand = new RegisterUserCommand();
            _registerUserCommandHandler = new RegisterUserCommand.RegisterUserCommandHandler(_userRepository.Object, _authService.Object);

            _fakeUsers = new()
            {
                new User
                {
                    Username = "Test",
                },
                new User
                {
                    Username = "Jack",
                    CitizenId = 98765432100,
                    Name = "Jack",
                    Surname = "Doe",
                    Email = "m@m.com",
                    MobilePhone = "5554443311",
                    Status = true,
                },
            };
        }

        [Test]
        public async Task RegisterUser_Success()
        {
            _registerUserCommand = new()
            {
                Username = "Test",
                CitizenId = 12345678900,
                Name = "John",
                Surname = "Doe",
                Email = "a@a.com",
                MobilePhone = "5554443322",
                Password = "123",
                ConfirmPassword = "123"
            };

            HashingHelper.CreatePasswordHash(_registerUserCommand.Password, out var passwordSalt, out var passwordHash);

            var user = new User
            {
                Username = _registerUserCommand.Username,
                CitizenId = _registerUserCommand.CitizenId,
                Name = _registerUserCommand.Name,
                Surname = _registerUserCommand.Surname,
                Email = _registerUserCommand.Email,
                MobilePhone = _registerUserCommand.MobilePhone,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            _userRepository.Setup(x => x.Query()).Returns(_fakeUsers.AsQueryable().BuildMock());
            _userRepository.Setup(x => x.Add(It.IsAny<User>())).Returns(user);

            _authService.Setup(x => x.GenerateToken(user)).Returns(new AuthResultDto("", DateTime.UtcNow));

            var result = await _registerUserCommandHandler.Handle(_registerUserCommand, CancellationToken.None);

            _userRepository.Verify(x => x.SaveChangesAsync());
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task RegisterUser_Fail_PasswordsDoNotMatch()
        {
            _registerUserCommand = new()
            {
                Username = "Test",
                CitizenId = 98765432100,
                Name = "John",
                Surname = "Doe",
                Email = "b@b.com",
                MobilePhone = "5550001122",
                Password = "123",
                ConfirmPassword = "321"
            };

            var result = await _registerUserCommandHandler.Handle(_registerUserCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
        } 
        
        [Test]
        public async Task RegisterUser_Fail_CitizenIdAlreadyExist()
        {
            _registerUserCommand = new()
            {
                Username = "Test",
                CitizenId = 98765432100,
                Name = "John",
                Surname = "Doe",
                Email = "b@b.com",
                MobilePhone = "5550001122",
                Password = "123",
                ConfirmPassword = "123"
            };
            _userRepository.Setup(x => x.Query()).Returns(_fakeUsers.AsQueryable().BuildMock());

            var result = await _registerUserCommandHandler.Handle(_registerUserCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
        } 
        
        [Test]
        public async Task RegisterUser_Fail_EmailAlreadyExist()
        {
            _registerUserCommand = new()
            {
                Username = "Test",
                CitizenId = 12345678900,
                Name = "John",
                Surname = "Doe",
                Email = "m@m.com",
                MobilePhone = "5550001122",
                Password = "123",
                ConfirmPassword = "123"
            };

            _userRepository.Setup(x => x.Query()).Returns(_fakeUsers.AsQueryable().BuildMock());

            var result = await _registerUserCommandHandler.Handle(_registerUserCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
        }
        
        [Test]
        public async Task RegisterUser_Fail_MobilPhoneAlreadyExist()
        {
            _registerUserCommand = new()
            {
                Username = "Test",
                CitizenId = 12345678900,
                Name = "John",
                Surname = "Doe",
                Email = "b@b.com",
                MobilePhone = "5554443311",
                Password = "123",
                ConfirmPassword = "123"
            };

            _userRepository.Setup(x => x.Query()).Returns(_fakeUsers.AsQueryable().BuildMock());

            var result = await _registerUserCommandHandler.Handle(_registerUserCommand, CancellationToken.None);
            result.Success.Should().BeFalse();
        }
    }
}

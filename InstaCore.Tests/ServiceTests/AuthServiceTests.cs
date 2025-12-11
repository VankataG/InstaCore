using InstaCore.Core.Contracts;
using InstaCore.Core.Contracts.Repos;
using InstaCore.Core.Dtos;
using InstaCore.Core.Exceptions;
using InstaCore.Core.Models;
using InstaCore.Core.Services;
using InstaCore.Core.Services.Contracts;
using Moq;
using NUnit.Framework;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using User = InstaCore.Core.Models.User;

namespace InstaCore.Tests.ServiceTests
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IPasswordHasher> passwordHasherMock;
        private Mock<IJwtTokenService> jwtTokenServiceMock;
        private IAuthService authService;

        [SetUp]
        public void SetUp()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            passwordHasherMock = new Mock<IPasswordHasher>();
            jwtTokenServiceMock = new Mock<IJwtTokenService>();
            this.authService = new AuthService(
                userRepositoryMock.Object,
                passwordHasherMock.Object,
                jwtTokenServiceMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

        [Test]
        public async Task LoginAsync_WithValidCredentials_ReturnsAuthResponse()
        {
            string testEmail = "user@example.com";
            LoginRequest request = new()
            {
                Email = testEmail,
                Password = "password123"
            };

            User user = new()
            {
                Id = Guid.NewGuid(),
                Username = "User",
                Email = testEmail,
                PasswordHash = "hashed",
                CreatedAt = DateTime.UtcNow,
            };

            this.userRepositoryMock
                .Setup(userRepo => userRepo.GetByEmailAsync(request.Email))
                .ReturnsAsync(user);

            this.passwordHasherMock
                .Setup(hasher => hasher.Verify(request.Password, user.PasswordHash))
                .Returns(true);

            this.jwtTokenServiceMock
                .Setup(jwt => jwt.CreateAccessToken(user, null))
                .Returns("test-token");

            var result = await authService.LoginAsync(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Token, Is.EqualTo("test-token"));
            Assert.That(result.Username, Is.EqualTo(user.Username));
        }

        [Test]
        public void LoginAsync_WithNoExistingUser_ThrowsUnauthorizedException()
        {
            LoginRequest request = new()
            {
                Email = "user@example.com",
                Password = "password123"
            };

            this.userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync(request.Email))
                .ReturnsAsync((User)null!);

            Assert.ThrowsAsync<UnauthorizedException>(async () => await authService.LoginAsync(request));
        }
        
        [Test]
        public void LoginAsync_WithWrongPassword_ThrowsUnauthorizedException()
        {
            LoginRequest request = new()
            {
                Email = "user@example.com",
                Password = "wrongPassword"
            };

            User user = new()
            {
                Id = Guid.NewGuid(),
                Username = "User",
                Email = request.Email,
                PasswordHash = "hashed",
                CreatedAt = DateTime.UtcNow,
            };

            this.userRepositoryMock
                .Setup(userRepo => userRepo.GetByEmailAsync(request.Email))
                .ReturnsAsync(user);

            this.passwordHasherMock
                .Setup(hasher => hasher.Verify(request.Password, user.PasswordHash))
                .Returns(false);

            Assert.ThrowsAsync<UnauthorizedException>(async () => await authService.LoginAsync(request));
        }

        [Test]
        public async Task RegisterAsync_WithValidData_CreatesUserReturnsAuthResponse()
        {
            RegisterRequest request = new()
            {
                Email = "user@example.com",
                Password = "password123",
                Username = "User"
            };

            this.userRepositoryMock
                .Setup(userRepo => userRepo.ExistsByEmailAsync(request.Email))
                .ReturnsAsync(false);

            this.userRepositoryMock
                .Setup(userRepo => userRepo.ExistsByUsernameAsync(request.Username))
                .ReturnsAsync(false);

            this.passwordHasherMock
                .Setup(hasher => hasher.Hash(request.Password))
                .Returns("hashed");

            this.jwtTokenServiceMock
                .Setup(jwt => jwt.CreateAccessToken(It.IsAny<User>(), null))
                .Returns("test-token");

            User? addedUser = null;
            this.userRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => addedUser = u);

            var result = await authService.RegisterAsync(request);

            Assert.That(addedUser, Is.Not.Null);
            Assert.That(addedUser!.Email, Is.EqualTo(request.Email));
            Assert.That(addedUser.Username, Is.EqualTo(request.Username));
            Assert.That(addedUser.PasswordHash, Is.EqualTo("hashed"));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Token, Is.EqualTo("test-token"));
            Assert.That(result.Username, Is.EqualTo(request.Username));
            Assert.That(result.UserId, Is.EqualTo(addedUser.Id));
        }

        [Test]
        public void RegisterAsync_WithExistingEmail_ThrowsConflictException()
        {
            RegisterRequest request = new()
            {
                Email = "user@example.com",
                Password = "password123",
                Username = "User"
            };

            this.userRepositoryMock
                .Setup(userRepo => userRepo.ExistsByEmailAsync(request.Email))
                .ReturnsAsync(true);

            Assert.ThrowsAsync<ConflictException>(async () => await authService.RegisterAsync(request));
        }

        [Test]
        public void RegisterAsync_WithExistingUsername_ThrowsConflictException()
        {
            RegisterRequest request = new()
            {
                Email = "user@example.com",
                Password = "password123",
                Username = "User"
            };

            this.userRepositoryMock
                .Setup(userRepo => userRepo.ExistsByUsernameAsync(request.Username))
                .ReturnsAsync(true);

            Assert.ThrowsAsync<ConflictException>(async () => await authService.RegisterAsync(request));
        }
    }
}

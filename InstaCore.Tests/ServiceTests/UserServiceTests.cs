using InstaCore.Core.Contracts.Repos;
using InstaCore.Core.Exceptions;
using InstaCore.Core.Models;
using InstaCore.Core.Services;
using InstaCore.Core.Services.Contracts;
using Moq;
using NUnit.Framework;

namespace InstaCore.Tests.ServiceTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IFollowRepository> followRepositoryMock;
        private IUserService userService;

        [SetUp]
        public void SetUp()
        {
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.followRepositoryMock = new Mock<IFollowRepository>();
            this.userService = new UserService(userRepositoryMock.Object, followRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

        [Test]
        public async Task GetByUsernameAsync_WithValidUsername_ReturnsUserResponse()
        {
            string username = "User";
            User user = new()
            {
                Username = username,
                Email = "user@example.com",
                PasswordHash = "hashed",
            };

            this.userRepositoryMock
                .Setup(repo => repo.GetByUsernameWithFollowsAsync(username))
                .ReturnsAsync(user);

            var result = await userService.GetByUsernameAsync(username, null);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo(user.Username));
            Assert.That(result.Id, Is.EqualTo(user.Id));
            Assert.That(result.IsFollowedByCurrentUser, Is.False);
        }

        [Test]
        public void GetByUsernameAsync_WithNotExistingUsername_ThrowsNotFoundException()
        {
            string username = "User";
            
            this.userRepositoryMock
                .Setup(repo => repo.GetByUsernameWithFollowsAsync(username))
                .ReturnsAsync((User)null!);

            Assert.ThrowsAsync<NotFoundException>(async () => await userService.GetByUsernameAsync(username, null));
        }

        [Test]
        public async Task GetMeAsync_WithValidId_ReturnsUserResponse()
        {
            Guid meGuid = Guid.NewGuid();
            User user = new()
            {
                Id = meGuid,
                Username = "User",
                Email = "user@example.com",
                PasswordHash = "hashed",
            };

            this.userRepositoryMock
                .Setup(repo => repo.GetByIdWithFollowsAsync(meGuid))
                .ReturnsAsync(user);

            var result = await userService.GetMeAsync(meGuid);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo(user.Username));
            Assert.That(result.Id, Is.EqualTo(meGuid));
            Assert.That(result.IsFollowedByCurrentUser, Is.False);
        }

        [Test]
        public void GetMeAsync_WithNotExistingId_ThrowsNotFoundException()
        {
            Guid meGuid = Guid.NewGuid();

            this.userRepositoryMock
                .Setup(repo => repo.GetByIdWithFollowsAsync(meGuid))
                .ReturnsAsync((User)null!);

            Assert.ThrowsAsync<NotFoundException>(async () => await userService.GetMeAsync(meGuid));
        }


    }
}

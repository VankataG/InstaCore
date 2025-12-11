using InstaCore.Core.Contracts.Repos;
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
    }
}

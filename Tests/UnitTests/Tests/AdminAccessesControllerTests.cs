using AccessControl.Api.Controllers;
using AccessControl.Api.Models.Entity;
using AccessControl.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.TestHelper;

namespace UnitTests.Tests
{
    [TestFixture]
    public class AdminAccessesControllerTests
    {
        private Mock<ILogger<AdminAccessesController>> _logger;
        private Mock<IAccessesRepository> _accessesRepository;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<AdminAccessesController>>();
            _accessesRepository = new Mock<IAccessesRepository>();
        }

        [Test]
        public async Task GetAllAccesses_WhenCalled_ReturnAllLocks()
        {
            //Arrange
            var allAccesse = Helpers.GetAllAccesses();
            _accessesRepository.Setup(lr => lr.GetAllAccessesAsync()).ReturnsAsync(allAccesse);
            var adminAccessesController = new AdminAccessesController(_accessesRepository.Object, _logger.Object);

            //Act
            var result = await adminAccessesController.GetAllAccesses();
            var actual = (result.Result as OkObjectResult).Value as IEnumerable<Access>;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual, Is.EquivalentTo(allAccesse));
            });

            _accessesRepository.Verify(ar => ar.GetAllAccessesAsync(), Times.Once);
        }

        [Test]
        public async Task GetByAccessorId_WhenCalled_ReturnAllAccessesRelatedToThisAccessor()
        {
            //Arrange
            var allAccesse = Helpers.GetAllAccesses();
            var accessor2 = Helpers.GetAccessor2();
            var accessor2Accesses = allAccesse.Where(x => x.AccessorId.Equals(accessor2.Id)).ToList();

            _accessesRepository.Setup(x => x.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Access, bool>>>())).ReturnsAsync(accessor2Accesses);
            var adminAccessesController = new AdminAccessesController(_accessesRepository.Object, _logger.Object);

            //Act
            var result = await adminAccessesController.GetByAccessorId(accessor2.Id);
            var actual = (result.Result as OkObjectResult).Value as IEnumerable<Access>;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual, Is.EquivalentTo(accessor2Accesses));
            });
        }

        [Test]
        public async Task GetByLockId_WhenCalled_ReturnAllAccessesRelatedToThisLock()
        {
            //Arrange
            var allAccesse = Helpers.GetAllAccesses();
            var officeLock = Helpers.GetOfficeLock();
            var officeLockAccesses = allAccesse.Where(x => x.LockId.ToString().Equals(officeLock.LockId.ToString())).ToList();

            _accessesRepository.Setup(x => x.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Access, bool>>>())).ReturnsAsync(officeLockAccesses);
            var adminAccessesController = new AdminAccessesController(_accessesRepository.Object, _logger.Object);

            //Act
            var result = await adminAccessesController.GetByLockId(officeLock.LockId.ToString());
            var actual = (result.Result as OkObjectResult).Value as IEnumerable<Access>;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual, Is.EquivalentTo(officeLockAccesses));
            });
        }
    }
}

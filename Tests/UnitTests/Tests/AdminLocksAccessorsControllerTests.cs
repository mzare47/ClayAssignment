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
    public class AdminLocksAccessorsControllerTests
    {
        private Mock<ILogger<AdminLocksAccessorsController>> _logger;
        private Mock<ILocksRepository> _locksRepository;
        private Mock<IAccessorsRepository> _accessorsRepository;
        private Mock<ILocksAccessorsRepository> _locksAccessorsRepository;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<AdminLocksAccessorsController>>();
            _locksRepository = new Mock<ILocksRepository>();
            _accessorsRepository = new Mock<IAccessorsRepository>();
            _locksAccessorsRepository = new Mock<ILocksAccessorsRepository>();
        }

        [Test]
        public async Task GetLockAccessors_WhenCalled_WithLockId_ReturnAllAccessorsThatAccessToThisLock()
        {
            //Arrange
            var officeLock = Helpers.GetOfficeLock();
            var officeLockAccessors = Helpers.GetLockAccessors(officeLock.LockId.ToString());
            _locksAccessorsRepository.Setup(lr => lr.GetLockAccessorsAsync(It.Is<string>(x => x.Equals(officeLock.LockId.ToString())))).ReturnsAsync(officeLockAccessors);
            var adminLocksAccessorsController = new AdminLocksAccessorsController(_locksAccessorsRepository.Object, _locksRepository.Object, _accessorsRepository.Object, _logger.Object);

            //Act
            var result = await adminLocksAccessorsController.GetLockAccessors(officeLock.LockId.ToString());
            var actual = (result.Result as OkObjectResult).Value as IEnumerable<ApplicationUser>;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual, Is.EquivalentTo(officeLockAccessors));
            });

            _locksAccessorsRepository.Verify(ar => ar.GetLockAccessorsAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GetAccessorLocks_WhenCalled_WithAccessorId_ReturnAllLocksThatThisAccessorsCanAccess()
        {
            //Arrange
            var accessor1 = Helpers.GetAccessor1();
            var accessor1Locks = Helpers.GetAccessorLocks(accessor1.Id);
            _locksAccessorsRepository.Setup(lr => lr.GetAccessorLocksAsync(It.Is<string>(x => x.Equals(accessor1.Id)))).ReturnsAsync(accessor1Locks);
            var adminLocksAccessorsController = new AdminLocksAccessorsController(_locksAccessorsRepository.Object, _locksRepository.Object, _accessorsRepository.Object, _logger.Object);

            //Act
            var result = await adminLocksAccessorsController.GetAccessorLocks(accessor1.Id);
            var actual = (result.Result as OkObjectResult).Value as IEnumerable<Lock>;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual, Is.EquivalentTo(accessor1Locks));
            });

            _locksAccessorsRepository.Verify(ar => ar.GetAccessorLocksAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task UpdateAccessorLocks_WhenCalled_WithAccessorIdAndLocksIds_AllLocksThatThisAccessorCanAccessUpdated()
        {
            //Arrange
            var allLocksAccessors = Helpers.GetAllLocksAccessors();

            var accessor1 = Helpers.GetAccessor1();
            var accessor1Locks = Helpers.GetAccessorLocks(accessor1.Id);
            var accessor1LocksIds = accessor1Locks.Select(x => x.LockId.ToString());

            var accessor1LocksToEdit = Helpers.GetAccessorLocks(accessor1.Id);
            accessor1LocksToEdit.Remove(accessor1Locks.First()); //Remove First Lock
            var accessor1LocksToEditIds = accessor1LocksToEdit.Select(x => x.LockId.ToString()).ToList();
            accessor1LocksToEditIds.Sort();

            _accessorsRepository.Setup(lr => lr.GetAccessorByIdAsync(It.IsAny<string>())).ReturnsAsync(accessor1);
            _locksRepository.Setup(lr => lr.GetLockByIdAsync(It.IsAny<string>())).ReturnsAsync((string x) => accessor1Locks.First(l => l.LockId.ToString().Equals(x)));
            _locksAccessorsRepository.Setup(lr => lr.GetByAccessorIdAsync(It.IsAny<string>())).ReturnsAsync((string aId) => allLocksAccessors.Where(x => x.AccessorId.Equals(aId)));
            _locksAccessorsRepository.Setup(lr => lr.DeleteRangeAsync(It.IsAny<IEnumerable<LockAccessor>>())).Callback<IEnumerable<LockAccessor>>((x) => allLocksAccessors.RemoveAll(a => x.Contains(a)));
            _locksAccessorsRepository.Setup(lr => lr.AddRangeAsync(It.IsAny<IEnumerable<LockAccessor>>())).Callback<IEnumerable<LockAccessor>>((x) => allLocksAccessors.AddRange(x)).ReturnsAsync((IEnumerable<LockAccessor> x) => x);

            var adminLocksAccessorsController = new AdminLocksAccessorsController(_locksAccessorsRepository.Object, _locksRepository.Object, _accessorsRepository.Object, _logger.Object);

            var expected = allLocksAccessors.Where(x => x.AccessorId.Equals(accessor1.Id));

            //Act
            var result = await adminLocksAccessorsController.UpdateAccessorLocks(accessor1.Id, accessor1LocksToEditIds);
            var actual = (result.Result as OkObjectResult).Value as IEnumerable<LockAccessor>;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual.OrderBy(x => x.LockAccessorId), Is.EquivalentTo(expected.OrderBy(x => x.LockAccessorId)));
            });

        }

        [Test]
        public async Task UpdateLockAccessors_WhenCalled_WithLockIdAndAccessorsIds_AllAccessorsCanAccessToThisLockUpdated()
        {
            //Arrange
            var allLocksAccessors = Helpers.GetAllLocksAccessors();

            var tunnelLock = Helpers.GetTunnelLock();
            var tunnelLockAccessors = Helpers.GetLockAccessors(tunnelLock.LockId.ToString());
            var tunnelLockAccessorsIds = tunnelLockAccessors.Select(x => x.Id);

            var tunnelLockAccessorsToEdit = Helpers.GetLockAccessors(tunnelLock.LockId.ToString());
            tunnelLockAccessorsToEdit.Remove(tunnelLockAccessors.First()); //Remove First Accessor
            var tunnelLockAccessorsToEditIds = tunnelLockAccessorsToEdit.Select(x => x.Id).ToList();
            tunnelLockAccessorsToEditIds.Sort();

            _locksRepository.Setup(lr => lr.GetLockByIdAsync(It.IsAny<string>())).ReturnsAsync(tunnelLock);
            _accessorsRepository.Setup(lr => lr.GetAccessorByIdAsync(It.IsAny<string>())).ReturnsAsync((string x) => tunnelLockAccessors.First(l => l.Id.Equals(x)));
            _locksAccessorsRepository.Setup(lr => lr.GetByLockIdAsync(It.IsAny<string>())).ReturnsAsync((string lId) => allLocksAccessors.Where(x => x.LockId.ToString().Equals(lId.ToString())));
            _locksAccessorsRepository.Setup(lr => lr.DeleteRangeAsync(It.IsAny<IEnumerable<LockAccessor>>())).Callback<IEnumerable<LockAccessor>>((x) => allLocksAccessors.RemoveAll(a => x.Contains(a)));
            _locksAccessorsRepository.Setup(lr => lr.AddRangeAsync(It.IsAny<IEnumerable<LockAccessor>>())).Callback<IEnumerable<LockAccessor>>((x) => allLocksAccessors.AddRange(x)).ReturnsAsync((IEnumerable<LockAccessor> x) => x);

            var adminLocksAccessorsController = new AdminLocksAccessorsController(_locksAccessorsRepository.Object, _locksRepository.Object, _accessorsRepository.Object, _logger.Object);

            var expected = allLocksAccessors.Where(x => x.LockId.ToString().Equals(tunnelLock.LockId.ToString()));

            //Act
            var result = await adminLocksAccessorsController.UpdateLockAccessors(tunnelLock.LockId.ToString(), tunnelLockAccessorsToEditIds);
            var actual = (result.Result as OkObjectResult).Value as IEnumerable<LockAccessor>;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual.OrderBy(x => x.LockAccessorId), Is.EquivalentTo(expected.OrderBy(x => x.LockAccessorId)));
            });
        }
    }
}

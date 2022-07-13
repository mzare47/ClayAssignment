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
    public class AdminLocksControllerTests
    {
        private Mock<ILogger<AdminLocksController>> _logger;
        private Mock<ILocksRepository> _locksRepository;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<AdminLocksController>>();
            _locksRepository = new Mock<ILocksRepository>();
        }

        [Test]
        public async Task GetAllLocks_WhenCalled_ReturnAllLocks()
        {
            //Arrange
            var allLocks = Helpers.GetAllLocks();
            _locksRepository.Setup(lr => lr.GetAllAsync()).ReturnsAsync(allLocks);
            var adminLocksController = new AdminLocksController(_locksRepository.Object, _logger.Object);

            //Act
            var result = await adminLocksController.GetAllLocks();
            var actual = (result.Result as OkObjectResult).Value as IEnumerable<Lock>;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual, Is.EquivalentTo(allLocks));
            });

            _locksRepository.Verify(ar => ar.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetLockById_WhenCalled_ReturnProperLock()
        {
            //Arrange
            var officeLock = Helpers.GetOfficeLock();
            var officeLockId = officeLock.LockId.ToString();
            _locksRepository.Setup(lr => lr.GetLockByIdAsync(It.Is<string>(x => x.Equals(officeLockId)))).ReturnsAsync(officeLock);
            var adminLocksController = new AdminLocksController(_locksRepository.Object, _logger.Object);

            //Act
            var result = await adminLocksController.GetLockById(officeLockId);
            var actual = (result.Result as OkObjectResult).Value as Lock;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual.LockId, Is.EqualTo(officeLock.LockId));
                Assert.That(actual.Name, Is.EqualTo(officeLock.Name));
                Assert.That(actual.Location, Is.EqualTo(officeLock.Location));
                Assert.That(actual.IsLocked, Is.EqualTo(officeLock.IsLocked));
                Assert.That(actual.AllowUnlocking, Is.EqualTo(officeLock.AllowUnlocking));
            });

            _locksRepository.Verify(ar => ar.GetLockByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task CreateLock_WhenCalled_LockSuccessfullyCreated()
        {
            //Arrange
            var lockToAdd = new Lock()
            {
                Name = "Test Lock",
                Location = "Test Lock Location",
                IsLocked = true,
                AllowUnlocking = true
            };

            var lockAdded = new Lock()
            {
                LockId = Guid.NewGuid(),
                Name = lockToAdd.Name,
                Location = lockToAdd.Location,
                IsLocked = lockToAdd.IsLocked,
                AllowUnlocking = lockToAdd.AllowUnlocking,
            };

            _locksRepository.Setup(lr => lr.AddAsync(It.IsAny<Lock>())).ReturnsAsync(lockAdded);
            var adminLocksController = new AdminLocksController(_locksRepository.Object, _logger.Object);

            //Act
            var result = await adminLocksController.CreateLock(lockToAdd);
            var actual = (result.Result as CreatedAtRouteResult).Value as Lock;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(actual.Name, Is.EqualTo(lockAdded.Name));
                Assert.That(actual.Location, Is.EqualTo(lockAdded.Location));
                Assert.That(actual.IsLocked, Is.EqualTo(lockAdded.IsLocked));
                Assert.That(actual.AllowUnlocking, Is.EqualTo(lockAdded.AllowUnlocking));
            });

            _locksRepository.Verify(ar => ar.AddAsync(It.IsAny<Lock>()), Times.Once);
        }

        [Test]
        public async Task UpdateLock_WhenCalled_LockSuccessfullyUpdated()
        {
            //Arrange
            var officeLock = Helpers.GetOfficeLock();
            var officeLockNameAfterUpdate = "Office Lock Updated";
            var officeLockId = officeLock.LockId.ToString();
            _locksRepository.Setup(lr => lr.GetLockByIdAsync(It.Is<string>(x => x.Equals(officeLockId)))).ReturnsAsync(officeLock);
            _locksRepository.Setup(lr => lr.UpdateAsync(It.IsAny<Lock>())).Callback(() => { officeLock.Name = officeLockNameAfterUpdate; return; });
            var adminLocksController = new AdminLocksController(_locksRepository.Object, _logger.Object);

            //Act
            var result = await adminLocksController.UpdateLock(officeLock);
            var actual = (result.Result as OkObjectResult).Value as Lock;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual.LockId, Is.EqualTo(officeLock.LockId));
                Assert.That(actual.Name, Is.EqualTo(officeLockNameAfterUpdate));
                Assert.That(actual.Location, Is.EqualTo(officeLock.Location));
                Assert.That(actual.IsLocked, Is.EqualTo(officeLock.IsLocked));
                Assert.That(actual.AllowUnlocking, Is.EqualTo(officeLock.AllowUnlocking));
            });

            _locksRepository.Verify(ar => ar.GetLockByIdAsync(It.IsAny<string>()), Times.Once);
            _locksRepository.Verify(ar => ar.UpdateAsync(It.IsAny<Lock>()), Times.Once);
        }

        [Test]
        public async Task DeleteLock_WhenCalled_LockSuccessfullyDeleted()
        {
            //Arrange
            var officeLock = Helpers.GetOfficeLock();
            var officeLockId = officeLock.LockId.ToString();
            _locksRepository.Setup(lr => lr.GetByIdAsync(It.Is<Guid>(x => x.ToString().Equals(officeLockId)))).ReturnsAsync(officeLock);
            _locksRepository.Setup(lr => lr.DeleteAsync(It.Is<Lock>(x => x.LockId.ToString() == officeLockId))).Callback(() => { return; });
            var adminLocksController = new AdminLocksController(_locksRepository.Object, _logger.Object);

            //Act
            var result = await adminLocksController.DeleteLock(officeLockId);
            var actual = (result.Result as OkObjectResult).Value as Lock;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual.LockId, Is.EqualTo(officeLock.LockId));
                Assert.That(actual.Name, Is.EqualTo(officeLock.Name));
                Assert.That(actual.Location, Is.EqualTo(officeLock.Location));
                Assert.That(actual.IsLocked, Is.EqualTo(officeLock.IsLocked));
                Assert.That(actual.AllowUnlocking, Is.EqualTo(officeLock.AllowUnlocking));
            });

            _locksRepository.Verify(ar => ar.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _locksRepository.Verify(ar => ar.DeleteAsync(It.IsAny<Lock>()), Times.Once);
        }
    }
}

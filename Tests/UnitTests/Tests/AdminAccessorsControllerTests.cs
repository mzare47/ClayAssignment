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
    public class AdminAccessorsControllerTests
    {
        private Mock<ILogger<AdminAccessorsController>> _logger;
        private Mock<IAccessorsRepository> _accessorsRepository;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<AdminAccessorsController>>();
            _accessorsRepository = new Mock<IAccessorsRepository>();
        }

        [Test]
        public async Task GetAllAccessors_WhenCalled_ReturnAllAccessors()
        {
            //Arrange
            var allAccessors = Helpers.GetAllAccessors();
            _accessorsRepository.Setup(lr => lr.GetAllAccessors()).ReturnsAsync(allAccessors);
            var adminAccessorsController = new AdminAccessorsController(_accessorsRepository.Object, _logger.Object);

            //Act
            var result = await adminAccessorsController.GetAllAccessors();
            var actual = (result.Result as OkObjectResult).Value as IEnumerable<ApplicationUser>;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual, Is.EquivalentTo(allAccessors));
            });

            _accessorsRepository.Verify(ar => ar.GetAllAccessors(), Times.Once);
        }

        [Test]
        public async Task GetAccessorById_WhenCalled_ReturnProperAccessor()
        {
            //Arrange
            var accessor1 = Helpers.GetAccessor1();
            _accessorsRepository.Setup(lr => lr.GetAccessorByIdAsync(It.Is<string>(x => x.Equals(accessor1.Id)))).ReturnsAsync(accessor1);
            var adminAccessorsController = new AdminAccessorsController(_accessorsRepository.Object, _logger.Object);

            //Act
            var result = await adminAccessorsController.GetAccessorById(accessor1.Id);
            var actual = (result.Result as OkObjectResult).Value as ApplicationUser;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual.Id, Is.EqualTo(accessor1.Id));
                Assert.That(actual.UserName, Is.EqualTo(accessor1.UserName));
                Assert.That(actual.Email, Is.EqualTo(accessor1.Email));
            });

            _accessorsRepository.Verify(ar => ar.GetAccessorByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task DeleteAccessor_WhenCalled_AccessorSuccessfullyDeleted()
        {
            //Arrange
            var accessor1 = Helpers.GetAccessor1();
            _accessorsRepository.Setup(lr => lr.GetAccessorByIdAsync(It.Is<string>(x => x.Equals(accessor1.Id)))).ReturnsAsync(accessor1);
            _accessorsRepository.Setup(lr => lr.DeleteAccessorAsync(It.Is<ApplicationUser>(x => x.Id == accessor1.Id))).Callback(() => { return; });
            var adminAccessorsController = new AdminAccessorsController(_accessorsRepository.Object, _logger.Object);

            //Act
            var result = await adminAccessorsController.DeleteAccessor(accessor1.Id);
            var actual = (result.Result as OkObjectResult).Value as ApplicationUser;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.TypeOf(typeof(OkObjectResult)));
                Assert.That(actual.Id, Is.EqualTo(accessor1.Id));
                Assert.That(actual.UserName, Is.EqualTo(accessor1.UserName));
                Assert.That(actual.Email, Is.EqualTo(accessor1.Email));
            });

            _accessorsRepository.Verify(ar => ar.GetAccessorByIdAsync(It.IsAny<string>()), Times.Once);
            _accessorsRepository.Verify(ar => ar.DeleteAccessorAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }
    }
}

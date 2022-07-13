using AccessControl.Api.Models.Entity;
using IntegrationTests.Factories;
using IntegrationTests.TestHelper;
using Shared.Lib.Extensions;
using System.Net.Http.Headers;

namespace IntegrationTests.Tests
{
    public class AdminAccessorsControllerTests : IClassFixture<AccessControlApiApplicationFactory>
    {
        private readonly AccessControlApiApplicationFactory _factory;
        private readonly string _adminUserName;
        private readonly string _adminPass;

        public AdminAccessorsControllerTests(AccessControlApiApplicationFactory factory)
        {
            _factory = factory;
            _adminUserName = "admin";
            _adminPass = "Admin*123";
        }

        [Fact]
        public async Task GetAllAccessors_WhenCalled_ReturnAllAccessors()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var allAccessor = Helpers.GetAllAccessors();
            var accessor1 = Helpers.GetAccessor1();
            var accessor2 = Helpers.GetAccessor2();

            //Act
            var response = await client.GetAsync($"/api/adminaccessors");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            var actual = await response.ReadContentAs<IEnumerable<ApplicationUser>>();
            Assert.Equal(allAccessor.Count, actual.Count());
            Assert.Single(actual.Where(a => a.Id.Equals(accessor1.Id) &&
                                            a.UserName.Equals(accessor1.UserName) &&
                                            a.Email.Equals(accessor1.Email)));
            Assert.Single(actual.Where(a => a.Id.Equals(accessor2.Id) &&
                                            a.UserName.Equals(accessor2.UserName) &&
                                            a.Email.Equals(accessor2.Email)));
        }

        [Fact]
        public async Task GetAccessorById_WhenCalled_ReturnProperAccessor()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var acceccor1 = Helpers.GetAccessor1();

            //Act
            var response = await client.GetAsync($"/api/adminaccessors/{acceccor1.Id}");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            var actual = await response.ReadContentAs<ApplicationUser>();
            Assert.Equal(acceccor1.Id, actual.Id);
            Assert.Equal(acceccor1.UserName, actual.UserName);
            Assert.Equal(acceccor1.Email, actual.Email);
        }

        [Fact]
        public async Task GetAccessorById_WhenCalled_WithInvalidAccessorIdReturnUnsuccessfulResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var invalidAccessorId = Guid.NewGuid().ToString();

            //Act
            var response = await client.GetAsync($"/api/adminaccessors/{invalidAccessorId}");

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteAccessor_WhenCalled_AccessorSuccessfullyDeleted()
        {
            //Arrange
            var acceccor1 = Helpers.GetAccessor1();
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            var deleteResponse = await client.DeleteAsync($"/api/adminaccessors/{acceccor1.Id}");
            var getByIdResponse = await client.GetAsync($"/api/adminaccessors/{acceccor1.Id}");

            //Assert
            Assert.NotNull(deleteResponse);
            deleteResponse.EnsureSuccessStatusCode();
            var actual = await deleteResponse.ReadContentAs<ApplicationUser>();
            Assert.Equal(acceccor1.Id, actual.Id);
            Assert.Equal(acceccor1.UserName, actual.UserName);
            Assert.Equal(acceccor1.Email, actual.Email);
            Assert.False(getByIdResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteAccessor_WhenCalled_WithInvalidAccessorId_ReturnUnsuccessfulResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var invalidAccessorId = Guid.NewGuid().ToString();

            //Act
            var response = await client.DeleteAsync($"/api/adminaccessors/{invalidAccessorId}");

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}

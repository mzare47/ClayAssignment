using AccessControl.Api.Models.Entity;
using IntegrationTests.Factories;
using IntegrationTests.TestHelper;
using Shared.Lib.Extensions;
using System.Net.Http.Headers;

namespace IntegrationTests.Tests
{
    [CollectionDefinition("Non-Parallel Collection", DisableParallelization = true)]
    [Collection("Non-Parallel Collection")]
    public class AdminLocksControllerTests : IClassFixture<AccessControlApiApplicationFactory>
    {
        private readonly AccessControlApiApplicationFactory _factory;
        private readonly string _adminUserName;
        private readonly string _adminPass;

        public AdminLocksControllerTests(AccessControlApiApplicationFactory factory)
        {
            _factory = factory;
            _adminUserName = "admin";
            _adminPass = "Admin*123";
        }

        [Fact]
        public async Task GetAllLocks_WhenCalled_ReturnAllLocks()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var allLocks = Helpers.GetAllLocks();
            var officeLock = Helpers.GetOfficeLock();
            var tunnelLock = Helpers.GetTunnelLock();

            //Act
            var response = await client.GetAsync($"/api/adminlocks");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            var actual = await response.ReadContentAs<IEnumerable<Lock>>();
            Assert.Equal(allLocks.Count, actual.Count());
            Assert.Single(actual.Where(a => a.LockId.Equals(officeLock.LockId) &&
                                            a.Name.Equals(officeLock.Name) &&
                                            a.IsLocked.Equals(officeLock.IsLocked) &&
                                            a.AllowUnlocking.Equals(officeLock.AllowUnlocking) &&
                                            a.Location.Equals(officeLock.Location)));
            Assert.Single(actual.Where(a => a.LockId.Equals(tunnelLock.LockId) &&
                                            a.Name.Equals(tunnelLock.Name) &&
                                            a.IsLocked.Equals(tunnelLock.IsLocked) &&
                                            a.AllowUnlocking.Equals(tunnelLock.AllowUnlocking) &&
                                            a.Location.Equals(tunnelLock.Location)));
        }

        [Fact]
        public async Task GetLockById_WhenCalled_ReturnProperLock()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var officeLock = Helpers.GetOfficeLock();

            //Act
            var response = await client.GetAsync($"/api/adminlocks/{officeLock.LockId}");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            var actual = await response.ReadContentAs<Lock>();
            Assert.Equal(officeLock.LockId, actual.LockId);
            Assert.Equal(officeLock.Name, actual.Name);
            Assert.Equal(officeLock.Location, actual.Location);
            Assert.Equal(officeLock.IsLocked, actual.IsLocked);
            Assert.Equal(officeLock.AllowUnlocking, actual.AllowUnlocking);
        }

        [Fact]
        public async Task GetLockById_WhenCalled_WithInvalidLockId_ReturnUnsuccessfulResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var invalidLockId = Guid.NewGuid().ToString();

            //Act
            var response = await client.GetAsync($"/api/adminlocks/{invalidLockId}");

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task CreateLock_WhenCalled_LockSuccessfullyCreated()
        {
            //Arrange
            var lockToAdd = new
            {
                Name = "Test Lock",
                Location = "Test Lock Location",
                IsLocked = true,
                AllowUnlocking = true
            };

            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            var response = await client.PostAsJson($"/api/adminlocks", lockToAdd);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            var actual = await response.ReadContentAs<Lock>();
            Assert.Equal(lockToAdd.Name, actual.Name);
            Assert.Equal(lockToAdd.Location, actual.Location);
            Assert.Equal(lockToAdd.IsLocked, actual.IsLocked);
            Assert.Equal(lockToAdd.AllowUnlocking, actual.AllowUnlocking);
        }

        [Fact]
        public async Task UpdateLock_WhenCalled_LockSuccessfullyUpdated()
        {
            //Arrange
            var officeLock = Helpers.GetOfficeLock();
            officeLock.Name = "Office Lock Updated";
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            var response = await client.PutAsJson($"/api/adminlocks", officeLock);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            var actual = await response.ReadContentAs<Lock>();
            Assert.Equal("Office Lock Updated", actual.Name);
        }

        [Fact]
        public async Task UpdateLock_WhenCalled_WithInvalidLock_ReturnUnsuccessfulResult()
        {
            //Arrange
            var lockToUpdate = new
            {
                LockId = Guid.NewGuid(),
                Name = "Invalid Lock",
                Location = "Invalid Lock Location",
                IsLocked = true,
                AllowUnlocking = true
            };

            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            var response = await client.PutAsJson($"/api/adminlocks", lockToUpdate);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteLock_WhenCalled_LockSuccessfullyDeleted()
        {
            //Arrange
            var officeLock = Helpers.GetOfficeLock();
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            var deleteResponse = await client.DeleteAsync($"/api/adminlocks/{officeLock.LockId}");
            var getByIdResponse = await client.GetAsync($"/api/adminlocks/{officeLock.LockId}");

            //Assert
            Assert.NotNull(deleteResponse);
            deleteResponse.EnsureSuccessStatusCode();
            var actual = await deleteResponse.ReadContentAs<Lock>();
            Assert.Equal(officeLock.LockId, actual.LockId);
            Assert.Equal(officeLock.Name, actual.Name);
            Assert.Equal(officeLock.Location, actual.Location);
            Assert.Equal(officeLock.IsLocked, actual.IsLocked);
            Assert.Equal(officeLock.AllowUnlocking, actual.AllowUnlocking);
            Assert.False(getByIdResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteLock_WhenCalled_WithInvalidLockId_ReturnUnsuccessfulResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var invalidLockId = Guid.NewGuid().ToString();

            //Act
            var response = await client.DeleteAsync($"/api/adminlocks/{invalidLockId}");

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}

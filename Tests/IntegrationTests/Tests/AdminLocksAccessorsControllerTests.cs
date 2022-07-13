using AccessControl.Api.Models.Entity;
using IntegrationTests.Factories;
using IntegrationTests.TestHelper;
using Shared.Lib.Extensions;
using System.Net.Http.Headers;

namespace IntegrationTests.Tests
{
    public class AdminLocksAccessorsControllerTests : IClassFixture<AccessControlApiApplicationFactory>
    {
        private readonly AccessControlApiApplicationFactory _factory;
        private readonly string _adminUserName;
        private readonly string _adminPass;

        public AdminLocksAccessorsControllerTests(AccessControlApiApplicationFactory factory)
        {
            _factory = factory;
            _adminUserName = "admin";
            _adminPass = "Admin*123";
        }

        [Fact]
        public async Task GetLockAccessors_WhenCalled_WithLockId_ReturnAllAccessorsThatAccessToThisLock()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var tunnelLock = Helpers.GetOfficeLock();
            var tunnelLockAccessors = Helpers.GetLockAccessors(tunnelLock.LockId.ToString());
            var tunnelLockAccessorsIds = tunnelLockAccessors.Select(x => x.Id).ToList();
            tunnelLockAccessorsIds.Sort();

            //Act
            var response = await client.GetAsync($"/api/adminlocksaccessors/getlockaccessors/{tunnelLock.LockId}");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            var actual = await response.ReadContentAs<IEnumerable<ApplicationUser>>();
            Assert.Equal(tunnelLockAccessors.Count, actual.Count());
            var actualAccessorsIds = actual.Select(x => x.Id).ToList();
            actualAccessorsIds.Sort();
            Assert.Equal(tunnelLockAccessorsIds, actualAccessorsIds);
        }

        [Fact]
        public async Task GetAccessorLocks_WhenCalled_WithAccessorId_ReturnAllLocksThatThisAccessorsCanAccess()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor1 = Helpers.GetAccessor1();
            var accessor1Locks = Helpers.GetAccessorLocks(accessor1.Id);
            var accessor1LocksIds = accessor1Locks.Select(x => x.LockId.ToString()).ToList();
            accessor1LocksIds.Sort();

            //Act
            var response = await client.GetAsync($"/api/adminlocksaccessors/getaccessorlocks/{accessor1.Id}");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            var actual = await response.ReadContentAs<IEnumerable<Lock>>();
            Assert.Equal(accessor1Locks.Count, actual.Count());
            var actualAccessorsIds = actual.Select(x => x.LockId.ToString()).ToList();
            actualAccessorsIds.Sort();
            Assert.Equal(accessor1LocksIds, actualAccessorsIds);
        }

        [Fact]
        public async Task UpdateAccessorLocks_WhenCalled_WithAccessorIdAndLocksIds_AllLocksThatThisAccessorCanAccessUpdated()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor1 = Helpers.GetAccessor1();
            var accessor1Locks = Helpers.GetAccessorLocks(accessor1.Id);
            accessor1Locks.Remove(accessor1Locks.First()); //Remove First Lock
            var accessor1LocksIds = accessor1Locks.Select(x => x.LockId.ToString()).ToList();
            accessor1LocksIds.Sort();

            //Act
            var response = await client.PatchAsJson($"/api/adminlocksaccessors/updateaccessorlocks/{accessor1.Id}", accessor1LocksIds);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            var actual = await response.ReadContentAs<IEnumerable<Lock>>();
            Assert.Equal(accessor1Locks.Count, actual.Count());
            var actualAccessorsIds = actual.Select(x => x.LockId.ToString()).ToList();
            actualAccessorsIds.Sort();
            Assert.Equal(accessor1LocksIds, actualAccessorsIds);
        }

        [Fact]
        public async Task UpdateAccessorLocks_WhenCalled_WithInvalidAccessorId_ReturnUnsuccessfulResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor1 = Helpers.GetAccessor1();
            var accessor1Locks = Helpers.GetAccessorLocks(accessor1.Id);
            accessor1Locks.Remove(accessor1Locks.First()); //Remove First Lock
            var accessor1LocksIds = accessor1Locks.Select(x => x.LockId.ToString()).ToList();
            accessor1LocksIds.Sort();

            var invalidAccessorId = Guid.NewGuid().ToString();

            //Act
            var response = await client.PatchAsJson($"/api/adminlocksaccessors/updateaccessorlocks/{invalidAccessorId}", accessor1LocksIds);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task UpdateAccessorLocks_WhenCalled_WithInvalidLocksIds_ReturnUnsuccessfulResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor1 = Helpers.GetAccessor1();
            var accessor1Locks = Helpers.GetAccessorLocks(accessor1.Id);
            accessor1Locks.Remove(accessor1Locks.First()); //Remove First Lock
            var accessor1LocksIds = accessor1Locks.Select(x => x.LockId.ToString()).ToList();
            accessor1LocksIds.Add(Guid.NewGuid().ToString()); //Add Invalid LockId
            accessor1LocksIds.Sort();

            //Act
            var response = await client.PatchAsJson($"/api/adminlocksaccessors/updateaccessorlocks/{accessor1.Id}", accessor1LocksIds);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task UpdateLockAccessors_WhenCalled_WithLockIdAndAccessorsIds_AllAccessorsCanAccessToThisLockUpdated()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var tunnelLock = Helpers.GetOfficeLock();
            var tunnelLockAccessors = Helpers.GetLockAccessors(tunnelLock.LockId.ToString());
            tunnelLockAccessors.Remove(tunnelLockAccessors.First()); //Remove First Accessor
            var tunnelLockAccessorsIds = tunnelLockAccessors.Select(x => x.Id).ToList();
            tunnelLockAccessorsIds.Sort();

            //Act
            var response = await client.PatchAsJson($"/api/adminlocksaccessors/updatelockaccessors/{tunnelLock.LockId}", tunnelLockAccessorsIds);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            var actual = await response.ReadContentAs<IEnumerable<ApplicationUser>>();
            Assert.Equal(tunnelLockAccessors.Count, actual.Count());
            var actualAccessorsIds = actual.Select(x => x.Id).ToList();
            actualAccessorsIds.Sort();
            Assert.Equal(tunnelLockAccessorsIds, actualAccessorsIds);
        }

        [Fact]
        public async Task UpdateLockAccessors_WhenCalled_WithInvalidLockId_ReturnUnsuccessfulResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var tunnelLock = Helpers.GetOfficeLock();
            var tunnelLockAccessors = Helpers.GetLockAccessors(tunnelLock.LockId.ToString());
            tunnelLockAccessors.Remove(tunnelLockAccessors.First()); //Remove First Accessor
            var tunnelLockAccessorsIds = tunnelLockAccessors.Select(x => x.Id).ToList();
            tunnelLockAccessorsIds.Sort();

            var invalidLockId = Guid.NewGuid().ToString();

            //Act
            var response = await client.PatchAsJson($"/api/adminlocksaccessors/updatelockaccessors/{invalidLockId}", tunnelLockAccessorsIds);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task UpdateLockAccessors_WhenCalled_WithInvalidAccessorsIds_ReturnUnsuccessfulResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var tunnelLock = Helpers.GetOfficeLock();
            var tunnelLockAccessors = Helpers.GetLockAccessors(tunnelLock.LockId.ToString());
            tunnelLockAccessors.Remove(tunnelLockAccessors.First()); //Remove First Accessor
            var tunnelLockAccessorsIds = tunnelLockAccessors.Select(x => x.Id).ToList();
            tunnelLockAccessorsIds.Sort();
            tunnelLockAccessorsIds.Add(Guid.NewGuid().ToString()); //Add Invalid AccessorId

            //Act
            var response = await client.PatchAsJson($"/api/adminlocksaccessors/updatelockaccessors/{tunnelLock.LockId}", tunnelLockAccessorsIds);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}

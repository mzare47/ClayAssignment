using AccessControl.Api.Models.Entity;
using IntegrationTests.Factories;
using IntegrationTests.TestHelper;
using Shared.Lib.Extensions;
using Shared.Lib.Models;
using System.Net;
using System.Net.Http.Headers;

namespace IntegrationTests.Tests
{
    [CollectionDefinition("Non-Parallel Collection", DisableParallelization = true)]
    [Collection("Non-Parallel Collection")]
    public class LocksControllerTests : IClassFixture<AccessControlApiApplicationFactory>
    {
        private readonly AccessControlApiApplicationFactory _factory;
        private readonly string _adminUserName;
        private readonly string _adminPass;
        private readonly string _accessor1UserName;
        private readonly string _accessor1Pass;
        private readonly string _accessor2UserName;
        private readonly string _accessor2Pass;
        private readonly string _accessor3UserName;
        private readonly string _accessor3Pass;

        public LocksControllerTests(AccessControlApiApplicationFactory factory)
        {
            _factory = factory;
            _adminUserName = "admin";
            _adminPass = "Admin*123";
            _accessor1UserName = "accessor1";
            _accessor1Pass = "Accessor1*123";
            _accessor2UserName = "accessor2";
            _accessor2Pass = "Accessor2*123";
            _accessor3UserName = "accessor3";
            _accessor3Pass = "Accessor3*123";
        }

        [Fact]
        public async Task UpdateLock_WhenCalled_WithInvalidAccessorId_ReturnUnsuccessfulResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_accessor1UserName, _accessor1Pass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var invalidAccessorId = $"{Guid.NewGuid()}";
            var officeLockId = Helpers.GetOfficeLock().LockId.ToString();
            var accessTyp = 1;

            //Act
            var response = await client.PutAsync($"/api/locks/{invalidAccessorId}/{officeLockId}/{accessTyp}", null);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task UpdateLock_WhenCalled_WithInvalidLockId_ReturnUnsuccessfulResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_accessor1UserName, _accessor1Pass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor1Id = Helpers.GetAccessor1().Id;
            var invalidLockId = $"{Guid.NewGuid()}";
            var accessTyp = 1;

            //Act
            var response = await client.PutAsync($"/api/locks/{accessor1Id}/{invalidLockId}/{accessTyp}", null);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task UpdateLock_WhenCalled_WithInvalidAccessType_ReturnUnsuccessfulResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_accessor1UserName, _accessor1Pass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor1Id = Helpers.GetAccessor1().Id;
            var officeLockId = Helpers.GetOfficeLock().LockId.ToString();
            var invalidAccessTyp = 10;

            //Act
            var response = await client.PutAsync($"/api/locks/{accessor1Id}/{officeLockId}/{invalidAccessTyp}", null);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task UpdateLock_WhenCalled_WithAccessorThatDoesntAccessAnyLocks_ReturnUnauthorizedResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_accessor3UserName, _accessor3Pass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor3Id = Helpers.GetAccessor3().Id;
            var officeLockId = Helpers.GetOfficeLock().LockId.ToString();
            var accessTyp = 1;

            //Act
            var response = await client.PutAsync($"/api/locks/{accessor3Id}/{officeLockId}/{accessTyp}", null);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task UpdateLock_WhenCalled_WithAccessorThatDoesntAccessAnyLocks_InsertUnauthorizedResultToAccesses()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor3Id = Helpers.GetAccessor3().Id;
            var officeLockId = Helpers.GetOfficeLock().LockId.ToString();
            var accessTyp = 1;

            //Act
            await client.PutAsync($"/api/locks/{accessor3Id}/{officeLockId}/{accessTyp}", null);
            var response = await client.GetAsync($"/api/adminaccesses/getbyaccessorid/{accessor3Id}");

            //Assert
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();
            var actual = (await response.ReadContentAs<IEnumerable<Access>>()).First();
            Assert.Equal(officeLockId, actual.LockId.ToString());
            Assert.Equal(accessor3Id, actual.AccessorId);
            Assert.False(actual.IsSuccessful);
            Assert.Equal("You are not allowed to open any locks", actual.Reason);
        }

        [Fact]
        public async Task UpdateLock_WhenCalled_WithAccessorThatDoesntAccessToLock_ReturnUnauthorizedResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_accessor2UserName, _accessor2Pass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor2Id = Helpers.GetAccessor2().Id;
            var officeLockId = Helpers.GetOfficeLock().LockId.ToString();
            var accessTyp = 1;

            //Act
            var response = await client.PutAsync($"/api/locks/{accessor2Id}/{officeLockId}/{accessTyp}", null);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task UpdateLock_WhenCalled_WithAccessorThatDoesntAccessToLock_InsertUnauthorizedResultToAccesses()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor2Id = Helpers.GetAccessor2().Id;
            var officeLock = Helpers.GetOfficeLock();
            var officeLockId = Helpers.GetOfficeLock().LockId.ToString();
            var accessTyp = 1;

            //Act
            await client.PutAsync($"/api/locks/{accessor2Id}/{officeLockId}/{accessTyp}", null);
            var response = await client.GetAsync($"/api/adminaccesses/getbyaccessorid/{accessor2Id}");

            //Assert
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();
            var actual = (await response.ReadContentAs<IEnumerable<Access>>()).First();
            Assert.Equal(officeLockId, actual.LockId.ToString());
            Assert.Equal(accessor2Id, actual.AccessorId);
            Assert.False(actual.IsSuccessful);
            Assert.Equal($"You are not allowed to open {officeLock.Name}", actual.Reason);
        }

        [Fact]
        public async Task UpdateLock_WhenCalled_WithAccessorToUnopenableLock_ReturnUnauthorizedResult()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_accessor2UserName, _accessor2Pass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor2Id = Helpers.GetAccessor2().Id;
            var unopenableLockId = Helpers.GetUnopenableLock().LockId.ToString();
            var accessTyp = (int)LockStatus.Unlock;

            //Act
            var response = await client.PutAsync($"/api/locks/{accessor2Id}/{unopenableLockId}/{accessTyp}", null);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task UpdateLock_WhenCalled_WithAccessorToUnopenableLock_InsertUnauthorizedResultToAccesses()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor2Id = Helpers.GetAccessor2().Id;
            var unopenableLock = Helpers.GetUnopenableLock();
            var unopenableLockId = Helpers.GetUnopenableLock().LockId.ToString();
            var accessTyp = 1;

            //Act
            await client.PutAsync($"/api/locks/{accessor2Id}/{unopenableLockId}/{accessTyp}", null);
            var response = await client.GetAsync($"/api/adminaccesses/getbylockid/{unopenableLockId}");

            //Assert
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();
            var actual = (await response.ReadContentAs<IEnumerable<Access>>()).First();
            Assert.Equal(unopenableLockId, actual.LockId.ToString());
            Assert.Equal(accessor2Id, actual.AccessorId);
            Assert.False(actual.IsSuccessful);
            Assert.Equal($"{unopenableLock.Name} is not Unlockable", actual.Reason);
        }

        [Fact]
        public async Task UpdateLock_WhenCalled_LockSuccessfullyUpdated()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_accessor1UserName, _accessor1Pass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor1Id = Helpers.GetAccessor1().Id;
            var tunnelLock = Helpers.GetTunnelLock();
            var tunnelLockId = tunnelLock.LockId.ToString();
            var accessTyp = (int)LockStatus.Unlock;

            //Act
            var response = await client.PutAsync($"/api/locks/{accessor1Id}/{tunnelLockId}/{accessTyp}", null);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            var actual = await response.ReadContentAs<Lock>();
            Assert.Equal(tunnelLock.LockId, actual.LockId);
            Assert.Equal(tunnelLock.Name, actual.Name);
            Assert.Equal(tunnelLock.Location, actual.Location);
            Assert.Equal(!tunnelLock.IsLocked, actual.IsLocked);
            Assert.Equal(tunnelLock.AllowUnlocking, actual.AllowUnlocking);
        }

        [Fact]
        public async Task UpdateLock_WhenCalled_InsertSuccessfulResultToAccesses()
        {
            //Arrange
            var client = await Helpers.GetHttpClientWithInitializedDb(_factory);

            var token = await Helpers.GetToken(_adminUserName, _adminPass, client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var accessor1Id = Helpers.GetAccessor1().Id;
            var tunnelLock = Helpers.GetTunnelLock();
            var tunnelLockId = tunnelLock.LockId.ToString();
            var accessTyp = 1;

            //Act
            await client.PutAsync($"/api/locks/{accessor1Id}/{tunnelLockId}/{accessTyp}", null);
            var response = await client.GetAsync($"/api/adminaccesses/getbylockid/{tunnelLockId}");

            //Assert
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode();
            var actual = (await response.ReadContentAs<IEnumerable<Access>>()).First();
            Assert.Equal(tunnelLockId, actual.LockId.ToString());
            Assert.Equal(accessor1Id, actual.AccessorId);
            Assert.True(actual.IsSuccessful);
            Assert.Null(actual.Reason);
        }
    }
}

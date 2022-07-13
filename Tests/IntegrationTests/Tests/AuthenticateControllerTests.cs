using AccessControl.Api.Models.Dto;
using IntegrationTests.Factories;
using IntegrationTests.TestHelper;
using Shared.Lib.Extensions;
using Shared.Lib.Helpers;
using Shared.Lib.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace IntegrationTests.Tests
{
    public class AuthenticateControllerTests : IClassFixture<AccessControlApiApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly string _adminUserName;
        private readonly string _adminPass;

        public AuthenticateControllerTests(AccessControlApiApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _adminUserName = "admin";
            _adminPass = "Admin*123";
        }

        [Fact]
        public async Task Login_WhenCalled_WithAuthorizedUser_Return_ValidToken()
        {
            //Arrange
            var user = new
            {
                Username = _adminUserName,
                Password = _adminPass
            };

            var admin = Helpers.GetAdmin();

            //Act
            var response = await _client.PostAsJson($"/api/authenticate/login", user);

            //Assert
            response.EnsureSuccessStatusCode();
            TokenDto token = await response.ReadContentAs<TokenDto>();
            List<Claim> claims = Helper.getClaimsFromToken(token.Token);
            Assert.NotNull(token);
            Assert.NotNull(token.Token);
            var uid = claims.First(c => c.Type.Equals("uid")).Value;
            Assert.Equal(admin.Id, uid);
            var name = claims.First(c => c.Type.Contains("name")).Value;
            Assert.Equal(user.Username, name);
            var role = claims.First(c => c.Type.Contains("role")).Value;
            Assert.Equal(UserRole.Admin.ToLower(), role.ToLower());

        }

        [Fact]
        public async Task Login_WhenCalled_WithUnauthorizedUser_Return_Unauthorized()
        {
            //Arrange
            var user = new
            {
                Username = "UnauthorizedUser",
                Password = "UnauthorizedUser*123"
            };

            //Act
            var response = await _client.PostAsJson($"/api/authenticate/login", user);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Register_WhenCalled_WithAuthorizedUser_UserSuccessfullyCreated()
        {
            //Arrange
            var user = new
            {
                Username = "testuser",
                Password = "Testuser*123",
                Email = "testuser@email.com",
                Role = "Accessor"
            };
            var token = await Helpers.GetToken(_adminUserName, _adminPass, _client);

            //Act

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsJson($"/api/authenticate/register", user);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            var actual = await response.ReadContentAs<Response>();
            Assert.Equal("Success", actual.Status);
            Assert.Equal("User created successfully!", actual.Message);
        }

        [Fact]
        public async Task Register_CannotBeCalled_WithAccessorRole()
        {
            //Arrange
            var user = new
            {
                Username = "testuser",
                Password = "Testuser*123",
                Email = "testuser@email.com",
                Role = "Accessor"
            };
            var token = await Helpers.GetToken("Accessor1", "Accessor1*123", _client);

            //Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.PostAsJson($"/api/authenticate/register", user);

            //Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

    }
}

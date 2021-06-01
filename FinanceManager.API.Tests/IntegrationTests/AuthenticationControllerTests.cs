using FinanceManager.API.Requests.v1;
using FinanceManager.API.Responses.v1;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinanceManager.API.Tests.IntegrationTests
{
    [Collection("IntegrationTest")]
    public class AuthenticationControllerTests : IntegrationTest
    {
        [Fact]
        public async Task RegisterUserAsync_WithInvalidData_ReturnsBadRequest()
        {
            var invalidData = new RegisterUserRequest { };
            var requestData = new StringContent(JsonConvert.SerializeObject(invalidData), Encoding.UTF8, "application/json");

            var response = await TestClient.PostAsync("api/v1/auth/register", requestData);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterUserAsync_WhenUserAlreadyExists_ReturnsBadRequest()
        {
            var registerUserRequest = new RegisterUserRequest { UserName = TestStandardUser.UserName, Email = TestStandardUser.Email, Password = "Newpassword123!" };
            var requestData = new StringContent(JsonConvert.SerializeObject(registerUserRequest), Encoding.UTF8, "application/json");
            await CreateUserDataAsync();

            var response = await TestClient.PostAsync("api/v1/auth/register", requestData);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterUserAsync_WhenUserDoesntExists_RegistersUserAndReturnsResponseWithTokens()
        {
            var registerUserRequest = new RegisterUserRequest { UserName = "Newtestuser1", Email = "Newtestuser1@gmail.com", Password = "Newpassword123!" };
            var requestData = new StringContent(JsonConvert.SerializeObject(registerUserRequest), Encoding.UTF8, "application/json");
            await CreateUserDataAsync();

            var response = await TestClient.PostAsync("api/v1/auth/register", requestData);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var authSucceededResponse = await response.Content.ReadAsAsync<AuthenticationSucceededResponse>();
            authSucceededResponse.Token.Should().NotBeNullOrEmpty();
            authSucceededResponse.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task LoginUserAsync_WithInvalidData_ReturnsBadRequest()
        {
            var invalidData = new LoginUserRequest { };
            var requestData = new StringContent(JsonConvert.SerializeObject(invalidData), Encoding.UTF8, "application/json");

            var response = await TestClient.PostAsync("api/v1/auth/login", requestData);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginUserAsync_WhenRequestDoesntMatchAnyUser_ReturnsBadRequest()
        {
            var loginUserRequest = new LoginUserRequest { UserName = "Newtestuser1", Password = "Newpassword123!" };
            var requestData = new StringContent(JsonConvert.SerializeObject(loginUserRequest), Encoding.UTF8, "application/json");
            await CreateUserDataAsync();

            var response = await TestClient.PostAsync("api/v1/auth/login", requestData);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginUserAsync_WhenUserDataMatchesUserInDatabase_ReturnsResponseWithTokens()
        {
            var loginUserRequest = new LoginUserRequest { UserName = TestStandardUser.UserName, Password = testUserPassword };
            var requestData = new StringContent(JsonConvert.SerializeObject(loginUserRequest), Encoding.UTF8, "application/json");
            await CreateUserDataAsync();

            var response = await TestClient.PostAsync("api/v1/auth/login", requestData);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var authSucceededResponse = await response.Content.ReadAsAsync<AuthenticationSucceededResponse>();
            authSucceededResponse.Token.Should().NotBeNullOrEmpty();
            authSucceededResponse.RefreshToken.Should().NotBeNullOrEmpty();
        }

    }
}

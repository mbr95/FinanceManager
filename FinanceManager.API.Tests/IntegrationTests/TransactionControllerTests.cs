using FinanceManager.API.Domain.Models;
using FinanceManager.API.Requests.v1;
using FinanceManager.API.Responses.v1;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinanceManager.API.Tests.IntegrationTests
{
    [Collection("IntegrationTest")]
    public class TransactionControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllTransactionsFromDatabaseForUser()
        {
            var transactions = new List<Transaction>()
            {
                new Transaction { Description = "zakupy1", Amount = 200, Date = DateTime.Now, CategoryId = TransactionCategoryId.Shopping},
                new Transaction { Description = "zakupy2", Amount = 400, Date = DateTime.Now, CategoryId = TransactionCategoryId.Shopping},
                new Transaction { Description = "zakupy3", Amount = 600, Date = DateTime.Now, CategoryId = TransactionCategoryId.Shopping}
            };
            var expectedResponse = new List<TransactionResponse>()
            {
                new TransactionResponse { Id = 1, Description = "zakupy1", Amount = 200, Date = transactions[0].Date, CategoryId = (int)TransactionCategoryId.Shopping},
                new TransactionResponse { Id = 2, Description = "zakupy2", Amount = 400, Date = transactions[1].Date, CategoryId = (int)TransactionCategoryId.Shopping},
                new TransactionResponse { Id = 3, Description = "zakupy3", Amount = 600, Date = transactions[2].Date, CategoryId = (int)TransactionCategoryId.Shopping}
            };
            await CreateUserDataAsync();            
            await AuthenticateAsync(TestAdmin);
            await AddDataToTransactionsDatabaseAsync(transactions);

            var response = await TestClient.GetAsync("api/v1/transactions");
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var transactionsResponse = await response.Content.ReadAsAsync<IEnumerable<TransactionResponse>>();
            transactionsResponse.Should().HaveCount(3);
            transactionsResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetAsync_WhenNoTransactionFound_ReturnsNotFound()
        {
            await CreateUserDataAsync();
            await AuthenticateAsync(TestStandardUser);

            var response = await TestClient.GetAsync("api/v1/transactions/1");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAsync_WhenUserIsNotTransactionOwner_ReturnsTransaction()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { Description = "zakupy1", Amount = 200, Date = DateTime.Now, CategoryId = TransactionCategoryId.Shopping }
            };
            await CreateUserDataAsync();
            await AuthenticateAsync(TestStandardUser);
            await AddDataToTransactionsDatabaseAsync(transactions);
            await AuthenticateAsync(TestAdmin);

            var response = await TestClient.GetAsync("api/v1/transactions/1");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAsync_WhenUserIsTransactionOwner_ReturnsTransaction()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { Description = "zakupy1", Amount = 200, Date = DateTime.Now, CategoryId = TransactionCategoryId.Shopping }
            };
            var expectedResponse = new TransactionResponse { Id = 1, Description = "zakupy1", Amount = 200, Date = transactions[0].Date, CategoryId = (int)TransactionCategoryId.Shopping };
            await CreateUserDataAsync();
            await AuthenticateAsync(TestStandardUser);
            await AddDataToTransactionsDatabaseAsync(transactions);

            var response = await TestClient.GetAsync("api/v1/transactions/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var transactionResponse = await response.Content.ReadAsAsync<TransactionResponse>();
            transactionResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task CreateAsync_WithInvalidData_ReturnsBadRequest()
        {
            var invalidData = new CreateTransactionRequest { };
            var requestData = new StringContent(JsonConvert.SerializeObject(invalidData), Encoding.UTF8, "application/json");
            await CreateUserDataAsync();
            await AuthenticateAsync(TestStandardUser);

            var response = await TestClient.PostAsync("api/v1/transactions", requestData);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateAsync_WithValidData_ReturnsCreated()
        {
            var validData = new CreateTransactionRequest { Description = "zakupy", Amount = -500, Date = DateTime.Now, CategoryId = (int)TransactionCategoryId.Shopping};
            var requestData = new StringContent(JsonConvert.SerializeObject(validData), Encoding.UTF8, "application/json");
            var expectedResponse = new TransactionResponse { Id = 1, Description = "zakupy", Amount = -500, Date = validData.Date, CategoryId = (int)TransactionCategoryId.Shopping };
            await CreateUserDataAsync();
            await AuthenticateAsync(TestStandardUser);

            var response = await TestClient.PostAsync("api/v1/transactions", requestData);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var createTransactionResponse = await response.Content.ReadAsAsync<TransactionResponse>();
            createTransactionResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task UpdateAsync_WhenUserIsNotTransactionOwner_ReturnBadRequest()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { Description = "zakupy1", Amount = 200, Date = DateTime.Now, CategoryId = TransactionCategoryId.Shopping }
            };
            var updateRequestData = new UpdateTransactionRequest { Description = "zakupy2", Amount = 500, Date = DateTime.Now, CategoryId = (int)TransactionCategoryId.Shopping };
            var requestData = new StringContent(JsonConvert.SerializeObject(updateRequestData), Encoding.UTF8, "application/json");
            await CreateUserDataAsync();
            await AuthenticateAsync(TestStandardUser);
            await AddDataToTransactionsDatabaseAsync(transactions);
            await AuthenticateAsync(TestAdmin);

            var response = await TestClient.PutAsync("api/v1/transactions/1", requestData);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAsync_WhenRequestDataIsInvalid_ReturnBadRequest()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { Description = "zakupy1", Amount = 200, Date = DateTime.Now, CategoryId = TransactionCategoryId.Shopping }
            };
            var updateRequestData = new UpdateTransactionRequest { };
            var requestData = new StringContent(JsonConvert.SerializeObject(updateRequestData), Encoding.UTF8, "application/json");
            await CreateUserDataAsync();
            await AuthenticateAsync(TestStandardUser);
            await AddDataToTransactionsDatabaseAsync(transactions);

            var response = await TestClient.PutAsync("api/v1/transactions/1", requestData);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAsync_WhenTransactionExists_ReturnOk()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { Description = "zakupy1", Amount = 200, Date = DateTime.Now, CategoryId = TransactionCategoryId.Shopping }
            };
            var updateRequestData = new UpdateTransactionRequest { Description = "zakupy2", Amount = 500, Date = DateTime.Now, CategoryId = (int)TransactionCategoryId.Entertainment };
            var expectedResponse = new TransactionResponse { Id = 1, Description = "zakupy2", Amount = 500, Date = updateRequestData.Date, CategoryId = (int)TransactionCategoryId.Entertainment };
            var requestData = new StringContent(JsonConvert.SerializeObject(updateRequestData), Encoding.UTF8, "application/json");
            await CreateUserDataAsync();
            await AuthenticateAsync(TestStandardUser);
            await AddDataToTransactionsDatabaseAsync(transactions);

            var response = await TestClient.PutAsync("api/v1/transactions/1", requestData);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var updateResponse = await response.Content.ReadAsAsync<TransactionResponse>();
            updateResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task DeleteAsync_WhenUserIsNotTransactionOwner_ReturnBadRequest()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { Description = "zakupy1", Amount = 200, Date = DateTime.Now, CategoryId = TransactionCategoryId.Shopping }
            };
            await CreateUserDataAsync();
            await AuthenticateAsync(TestStandardUser);
            await AddDataToTransactionsDatabaseAsync(transactions);
            await AuthenticateAsync(TestAdmin);

            var response = await TestClient.DeleteAsync("api/v1/transactions/1");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteAsync_WhenUserIsTransactionOwner_ReturnNoContent()
        {
            var transactions = new List<Transaction>
            {
                new Transaction { Description = "zakupy1", Amount = 200, Date = DateTime.Now, CategoryId = TransactionCategoryId.Shopping }
            };
            await CreateUserDataAsync();
            await AuthenticateAsync(TestStandardUser);
            await AddDataToTransactionsDatabaseAsync(transactions);

            var response = await TestClient.DeleteAsync("api/v1/transactions/1");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}

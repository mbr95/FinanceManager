using FinanceManager.API.Domain.Models;
using FinanceManager.API.Responses.v1;
using FluentAssertions;
using Microsoft.Extensions.Localization.Internal;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace FinanceManager.API.IntegrationTests
{
    public class CategoryControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAllAsync_WithNoCategories_ReturnsEmptyResponse()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync("api/v1/categories");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await DeserializeResponse<List<TransactionCategoryResponse>>(response)).Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_WhenCategoriesExist_ReturnsExistingCategories()
        {
            // Arrange
            var categories = new List<TransactionCategory>()
            {
                new TransactionCategory {Id = TransactionCategoryId.Entertainment, Name = TransactionCategoryId.Entertainment.ToString()},
                new TransactionCategory {Id = TransactionCategoryId.Education, Name = TransactionCategoryId.Education.ToString()},
                new TransactionCategory {Id = TransactionCategoryId.Health, Name = TransactionCategoryId.Health.ToString()},
                new TransactionCategory {Id = TransactionCategoryId.Investments, Name = TransactionCategoryId.Investments.ToString()},
            };
            var expectedResponse = new List<TransactionCategoryResponse>()
            {
                new TransactionCategoryResponse {Id = (int)TransactionCategoryId.Entertainment, Name = TransactionCategoryId.Entertainment.ToString()},
                new TransactionCategoryResponse {Id = (int)TransactionCategoryId.Education, Name = TransactionCategoryId.Education.ToString()},
                new TransactionCategoryResponse {Id = (int)TransactionCategoryId.Health, Name = TransactionCategoryId.Health.ToString()},
                new TransactionCategoryResponse {Id = (int)TransactionCategoryId.Investments, Name = TransactionCategoryId.Investments.ToString()},
            };
            await InsertCategoriesIntoDatabase(categories);
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync("api/v1/categories");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await DeserializeResponse<List<TransactionCategoryResponse>>(response)).Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetAsync_WhenCategoryDoesntExist_ReturnNotFound()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync("api/v1/categories/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public async Task GetAsync_WhenCategoryExists_ReturnCategoryResponse(int categoryId)
        {
            // Arrange
            var categories = new List<TransactionCategory>()
            {
                new TransactionCategory {Id = TransactionCategoryId.Salary, Name = TransactionCategoryId.Salary.ToString()},
                new TransactionCategory {Id = TransactionCategoryId.Investments, Name = TransactionCategoryId.Investments.ToString()},
                new TransactionCategory {Id = TransactionCategoryId.Returns, Name = TransactionCategoryId.Returns.ToString()},
                new TransactionCategory {Id = TransactionCategoryId.Sales, Name = TransactionCategoryId.Sales.ToString()},
                new TransactionCategory {Id = TransactionCategoryId.Shopping, Name = TransactionCategoryId.Shopping.ToString()},
                new TransactionCategory {Id = TransactionCategoryId.Entertainment, Name = TransactionCategoryId.Entertainment.ToString()}
            };
            var expectedCategoryResponse = new TransactionCategoryResponse { Id = (int)categories[categoryId-1].Id, Name = categories[categoryId-1].Id.ToString() };
            await InsertCategoriesIntoDatabase(categories);
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync($"api/v1/categories/{categoryId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await DeserializeResponse<TransactionCategoryResponse>(response)).Should().BeEquivalentTo(expectedCategoryResponse);
        }
    }
}

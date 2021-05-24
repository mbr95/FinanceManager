using FinanceManager.API.Domain.Models;
using FinanceManager.API.Responses.v1;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FinanceManager.API.Tests.IntegrationTests
{
    public class CategoryControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllPredefinedCategories()
        {
            await AuthenticateAsync();

            var response = await TestClient.GetAsync("api/v1/categories");
            var categoriesResponse = await response.Content.ReadAsAsync<IEnumerable<TransactionCategoryResponse>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            categoriesResponse.Count().Should().Be(Enum.GetNames(typeof(TransactionCategoryId)).Length);
        }

        [Theory]
        [InlineData(14)]
        [InlineData(18)]
        [InlineData(46)]
        [InlineData(76)]
        public async Task GetAllAsync_WhenCategoryDoesntExist_ReturnsNotFound(int categoryId)
        {
            await AuthenticateAsync();

            var response = await TestClient.GetAsync($"api/v1/categories/{categoryId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        public async Task GetAsync_WhenCategoryExists_ReturnsChosenCategory(int categoryId)
        {
            var expectedCategory = new TransactionCategoryResponse { Id = categoryId, Name = ((TransactionCategoryId)categoryId).ToString() }; 
            await AuthenticateAsync();

            var response = await TestClient.GetAsync($"api/v1/categories/{categoryId}");
            var categoryResponse = await response.Content.ReadAsAsync<TransactionCategoryResponse>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            categoryResponse.Should().BeEquivalentTo(expectedCategory);
        }
    }
}

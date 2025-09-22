using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refit;
using HierarchicalDirectory.RefitClient;
using HierarchicalDirectory.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace HierarchicalDirectory.IntegrationTests
{
    [TestClass]
    public class CategoryApiIntegrationTests
    {
        private ICategoryApi _client;
        private const string BaseUrl = "http://localhost:5000"; // Измените при необходимости

        [TestInitialize]
        public void Setup()
        {
            _client = RestService.For<ICategoryApi>(BaseUrl);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsCategories()
        {
            var result = await _client.GetAllAsync();
            Assert.IsNotNull(result);
            Assert.IsTrue(result is IEnumerable<CategoryDto>);
        }

        [TestMethod]
        public async Task CreateAndGetCategory_WorksCorrectly()
        {
            var dto = new CategoryDto { Name = "IntegrationTestCategory" };
            var created = await _client.CreateAsync(dto);
            Assert.IsNotNull(created);
            Assert.AreEqual("IntegrationTestCategory", created.Name);
            var fetched = await _client.GetByIdAsync(created.Id);
            Assert.IsNotNull(fetched);
            Assert.AreEqual(created.Id, fetched.Id);
        }

        [TestMethod]
        public async Task UpdateCategory_WorksCorrectly()
        {
            var dto = new CategoryDto { Name = "ToUpdate" };
            var created = await _client.CreateAsync(dto);
            var updateDto = new CategoryDto { Name = "UpdatedName" };
            var updated = await _client.UpdateAsync(created.Id, updateDto);
            Assert.IsNotNull(updated);
            Assert.AreEqual("UpdatedName", updated.Name);
        }

        [TestMethod]
        public async Task DeleteCategory_WorksCorrectly()
        {
            var dto = new CategoryDto { Name = "ToDelete" };
            var created = await _client.CreateAsync(dto);
            await _client.DeleteAsync(created.Id);
            var deleted = await _client.GetByIdAsync(created.Id);
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public async Task CreateCategory_WithChildren_WorksCorrectly()
        {
            var parent = await _client.CreateAsync(new CategoryDto { Name = "Parent" });
            var child = await _client.CreateAsync(new CategoryDto { Name = "Child", ParentId = parent.Id });
            var fetchedParent = await _client.GetByIdAsync(parent.Id, true);
            Assert.IsNotNull(fetchedParent.Children);
            Assert.IsTrue(fetchedParent.Children.Any(c => c.Id == child.Id));
        }

        [TestMethod]
        public async Task GetAllAsync_WithDepthAndSearch_WorksCorrectly()
        {
            var parent = await _client.CreateAsync(new CategoryDto { Name = "SearchParent" });
            var child = await _client.CreateAsync(new CategoryDto { Name = "SearchChild", ParentId = parent.Id });
            var result = await _client.GetAllAsync(1, "SearchParent");
            Assert.IsTrue(result.Any(c => c.Name == "SearchParent"));
            Assert.IsFalse(result.Any(c => c.Name == "SearchChild"));
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistent_ReturnsNullOrNotFound()
        {
            var result = await _client.GetByIdAsync("non-existent-id");
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateCategory_NonExistent_ThrowsException()
        {
            var updateDto = new CategoryDto { Name = "ShouldFail" };
            await Assert.ThrowsExceptionAsync<ApiException>(async () => await _client.UpdateAsync("non-existent-id", updateDto));
        }

        [TestMethod]
        public async Task DeleteCategory_NonExistent_DoesNotThrow()
        {
            // Ожидается успешное выполнение (NoContent), даже если категории нет
            await _client.DeleteAsync("non-existent-id");
        }

        [TestMethod]
        public async Task CreateCategory_WithDataAndSchema_WorksCorrectly()
        {
            var dto = new CategoryDto
            {
                Name = "WithData",
                Data = new { productCode = "ABC123", value = 42 },
                Schema = new { type = "object", properties = new { productCode = new { type = "string" }, value = new { type = "number" } } }
            };
            var created = await _client.CreateAsync(dto);
            Assert.IsNotNull(created);
            Assert.AreEqual("WithData", created.Name);
            Assert.IsNotNull(created.Data);
        }
    }
}

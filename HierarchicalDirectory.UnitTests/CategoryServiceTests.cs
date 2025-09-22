using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HierarchicalDirectory.Application;
using HierarchicalDirectory.Domain;
using HierarchicalDirectory.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace HierarchicalDirectory.UnitTests
{
    [TestClass]
    public class CategoryServiceTests
    {
        private Mock<IRepository<Category>> _repoMock;
        private CategoryService _service;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IRepository<Category>>();
            _service = new CategoryService(_repoMock.Object);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsRootCategories()
        {
            var categories = new List<Category>
            {
                new Category { Id = "1", Name = "Root", ParentId = null },
                new Category { Id = "2", Name = "Child", ParentId = "1" }
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);
            var result = await _service.GetAllAsync();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Root", result.First().Name);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsCategoryDto()
        {
            var category = new Category { Id = "1", Name = "Test", ParentId = null };
            _repoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(category);
            var result = await _service.GetByIdAsync("1");
            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.Name);
        }

        [TestMethod]
        public async Task CreateAsync_ValidCategory_ReturnsDto()
        {
            var dto = new CategoryDto { Name = "NewCat" };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Category>());
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);
            // Мок для валидации (категория родителя)
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Category { Id = "parent", Name = "Parent" });
            var result = await _service.CreateAsync(dto);
            Assert.IsNotNull(result);
            Assert.AreEqual("NewCat", result.Name);
        }

        [TestMethod]
        public async Task UpdateAsync_ExistingCategory_UpdatesAndReturnsDto()
        {
            var category = new Category { Id = "1", Name = "OldName", ParentId = "parent" };
            _repoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(category);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);
            // Мок для валидации (категория родителя)
            _repoMock.Setup(r => r.GetByIdAsync("parent")).ReturnsAsync(new Category { Id = "parent", Name = "Parent" });
            var dto = new CategoryDto { Name = "UpdatedName" };
            var result = await _service.UpdateAsync("1", dto);
            Assert.IsNotNull(result);
            Assert.AreEqual("UpdatedName", result.Name);
        }

        [TestMethod]
        public async Task DeleteAsync_DeletesCategory()
        {
            _repoMock.Setup(r => r.DeleteAsync("1")).Returns(Task.CompletedTask).Verifiable();
            await _service.DeleteAsync("1");
            _repoMock.Verify(r => r.DeleteAsync("1"), Times.Once);
        }

        [TestMethod]
        public async Task CreateAsync_InvalidValidation_ThrowsException()
        {
            var dto = new CategoryDto { Name = "NewCat" };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Category>());
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);
            // Мок для валидации (категория не найдена)
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Category)null);
            await Assert.ThrowsExceptionAsync<System.Exception>(async () => await _service.CreateAsync(dto));
        }

        [TestMethod]
        public async Task UpdateAsync_InvalidValidation_ThrowsException()
        {
            var category = new Category { Id = "1", Name = "OldName", ParentId = "parent" };
            _repoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(category);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);
            // Мок для валидации (категория не найдена)
            _repoMock.Setup(r => r.GetByIdAsync("parent")).ReturnsAsync((Category)null);
            var dto = new CategoryDto { Name = "UpdatedName" };
            await Assert.ThrowsExceptionAsync<System.Exception>(async () => await _service.UpdateAsync("1", dto));
        }

        [TestMethod]
        public void CategoryDto_Properties_SetAndGet()
        {
            var dto = new CategoryDto
            {
                Id = "1",
                Name = "Test",
                ParentId = null,
                Data = null,
                Schema = null,
                IsLatest = true,
                LoadDate = System.DateTime.Now,
                Children = null
            };
            Assert.AreEqual("1", dto.Id);
            Assert.AreEqual("Test", dto.Name);
            Assert.IsTrue(dto.IsLatest);
        }
    }
}

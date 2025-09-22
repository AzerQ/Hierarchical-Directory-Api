using Microsoft.AspNetCore.Mvc;
using HierarchicalDirectory.Application;
using HierarchicalDirectory.Models;

namespace HierarchicalDirectory.AspNetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        /// <summary>
        /// Конструктор контроллера категорий.
        /// </summary>
        /// <param name="service">Сервис для работы с категориями</param>
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Получить все категории (с возможностью фильтрации по глубине и поиску).
        /// </summary>
        /// <param name="depth">Глубина вложенности</param>
        /// <param name="search">Строка поиска по имени</param>
        /// <returns>Список категорий</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll([FromQuery] int? depth = null, [FromQuery] string? search = null)
        {
            var result = await _service.GetAllAsync(depth, search);
            return Ok(result);
        }

        /// <summary>
        /// Получить категорию по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <param name="includeChildren">Включать дочерние элементы</param>
        /// <returns>Категория или 404, если не найдена</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(string id, [FromQuery] bool includeChildren = false)
        {
            var result = await _service.GetByIdAsync(id, includeChildren);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Создать новую категорию.
        /// </summary>
        /// <param name="dto">Данные категории</param>
        /// <returns>Созданная категория</returns>
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CategoryDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Обновить существующую категорию.
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <param name="dto">Новые данные категории</param>
        /// <returns>Обновленная категория или 404, если не найдена</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> Update(string id, [FromBody] CategoryDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Удалить категорию по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <returns>Статус выполнения операции</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

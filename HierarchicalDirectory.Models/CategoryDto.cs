using System;
using System.Collections.Generic;

namespace HierarchicalDirectory.Application
{
    /// <summary>
    /// DTO для передачи данных категории между слоями приложения.
    /// </summary>
    public class CategoryDto
    {
        /// <summary>Уникальный идентификатор категории</summary>
        public string Id { get; set; }
        /// <summary>Название категории</summary>
        public string Name { get; set; }
        /// <summary>Идентификатор родительской категории (если есть)</summary>
        public string? ParentId { get; set; }
        /// <summary>Данные категории (JSON)</summary>
        public object? Data { get; set; }
        /// <summary>Схема данных категории (JSON Schema)</summary>
        public object? Schema { get; set; }
        /// <summary>Признак самой актуальной версии</summary>
        public bool IsLatest { get; set; }
        /// <summary>Дата загрузки/создания категории</summary>
        public DateTime LoadDate { get; set; }
        /// <summary>Дочерние категории</summary>
        public List<CategoryDto>? Children { get; set; }
    }
}

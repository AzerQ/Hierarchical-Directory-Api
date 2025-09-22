using System;
using System.Collections.Generic;

namespace HierarchicalDirectory.Domain
{
    /// <summary>
    /// Сущность категории для иерархического справочника.
    /// </summary>
    public class Category
    {
        /// <summary>Уникальный идентификатор категории</summary>
        public string Id { get; set; }
        /// <summary>Название категории</summary>
        public string Name { get; set; }
        /// <summary>Идентификатор родительской категории (если есть)</summary>
        public string? ParentId { get; set; }
        /// <summary>Данные категории в виде строки (JSON)</summary>
        public string? Data { get; set; }
        /// <summary>Схема данных категории (JSON Schema)</summary>
        public string? Schema { get; set; }
        /// <summary>Признак самой актуальной версии</summary>
        public bool IsLatest { get; set; } = true;
        /// <summary>Дата загрузки/создания категории</summary>
        public DateTime LoadDate { get; set; }
        /// <summary>Родительская категория</summary>
        public virtual Category Parent { get; set; }
        /// <summary>Коллекция дочерних категорий</summary>
        public virtual ICollection<Category> Children { get; set; } = new List<Category>();
    }
}

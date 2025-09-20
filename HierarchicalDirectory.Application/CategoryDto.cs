using System;
using System.Collections.Generic;

namespace HierarchicalDirectory.Application
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ParentId { get; set; }
        public object? Data { get; set; }
        public object? Schema { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LoadDate { get; set; }
        public List<CategoryDto>? Children { get; set; }
    }
}

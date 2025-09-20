using System;
using System.Collections.Generic;

namespace HierarchicalDirectory.Domain
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ParentId { get; set; }
        public string? Data { get; set; }
        public string? Schema { get; set; }
        public bool IsLatest { get; set; } = true;
        public DateTime LoadDate { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; } = new List<Category>();
    }
}
